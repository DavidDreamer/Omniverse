using Dreambox.Rendering.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using RenderSettings = Omniverse.Rendering.RenderSettings;

namespace Omniverse.Mapping
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial class MinimapRenderSystem : SystemBase
	{
		private static class ShaderPass
		{
			public const int FogOfWar = 0;
			public const int Frustrum = 1;
		}

		private static class ShaderVariables
		{
			public static int MapProperties { get; } = Shader.PropertyToID(nameof(MapProperties));
		}

		private readonly struct VirtualCamera
		{
			public readonly Matrix4x4 WorldToCameraMatrix;

			public readonly Matrix4x4 ProjectionMatrix;

			public VirtualCamera(int mapSize)
			{
				Vector3 position = new(0, 10, 0);
				Quaternion rotation = Quaternion.LookRotation(Vector3.down);
				Vector3 size = new(1, 1, -1);

				WorldToCameraMatrix = Matrix4x4.TRS(position, rotation, size).inverse;

				const float nearPlane = 0.3f;
				const float farPlane = 1000;

				ProjectionMatrix = Matrix4x4.Ortho(-mapSize, mapSize, -mapSize, mapSize, nearPlane, farPlane);
			}
		}

		private MapSettings MapSettings { get; set; }

		private MinimapRenderSettings Settings;

		public RenderTexture RenderTexture { get; private set; }

		private ConstantComputeBuffer<MapShaderProperties> PropertiesBuffer { get; set; }

		private VirtualCamera Camera { get; set; }

		private MinimapUnitDrawer MinimapUnitDrawer { get; set; }

		protected override void OnCreate()
		{
			RequireForUpdate<MapSettings>();
			RequireForUpdate<RenderSettings>();
		}

		protected override void OnStartRunning()
		{
			RenderPipelineManager.beginContextRendering += OnBeginContextRendering;

			MapSettings = SystemAPI.GetSingleton<MapSettings>();

			RenderTexture = new RenderTexture(
				MapSettings.Size.x,
				MapSettings.Size.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			var renderSettings = SystemAPI.GetSingleton<RenderSettings>();
			Settings = renderSettings.Minimap;

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(MapSettings.Size.x, MapSettings.Size.y, 0, 0)
			};

			PropertiesBuffer = new ConstantComputeBuffer<MapShaderProperties>(ShaderVariables.MapProperties);
			PropertiesBuffer.SetData(mapShaderProperties);

			Camera = new VirtualCamera(MapSettings.Size.x / 2);

			MinimapUnitDrawer = new(Settings.MeshDrawSettings, 64);
		}

		protected override void OnStopRunning()
		{
			RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;

			RenderTexture.Release();
			PropertiesBuffer.Dispose();
		}

		protected override void OnUpdate()
		{
		}

		private void OnBeginContextRendering(ScriptableRenderContext context, System.Collections.Generic.List<Camera> arg2)
		{
			using var scope = new CommandBufferContextScope(context, "Minimap");

			CommandBuffer commandBuffer = scope.CommandBuffer;

			commandBuffer.SetViewProjectionMatrices(Camera.WorldToCameraMatrix, Camera.ProjectionMatrix);

			commandBuffer.SetRenderTarget(RenderTexture);
			commandBuffer.ClearRenderTarget(true, true, Color.clear);

			DrawUnits();
			DrawFogOfWar();
			DrawFrustrum();

			void DrawUnits()
			{
				//TODO remove check
				if (!SystemAPI.HasSingleton<Player>())
				{
					return;
				}

				var player = SystemAPI.GetSingleton<Player>();

				foreach ((var unit, var localTransform, var faction, var entity) in SystemAPI.Query<Unit, LocalTransform, Faction>().WithEntityAccess())
				{
					float4x4 matrix = localTransform.ToMatrix();
					Color tint = player.FactionID == faction.ID ? Color.green : Color.red;
					MinimapUnitDrawer.Draw(commandBuffer, matrix, tint);
				}

				MinimapUnitDrawer.Flush(commandBuffer);
			}

			void DrawFogOfWar()
			{
				if (!SystemAPI.HasSingleton<FogOfWar>())
				{
					return;
				}

				Blitter.BlitTexture(commandBuffer, RenderTexture, new Vector4(1, 1, 0, 0), Settings.FrustrumMaterial, ShaderPass.FogOfWar);
			}

			void DrawFrustrum()
			{
				var camera = UnityEngine.Camera.main;
				var plane = new Plane(Vector3.up, 0);

				var point1 = Point(new Vector3(0, 0, 0));
				var point2 = Point(new Vector3(0, 1, 0));
				var point3 = Point(new Vector3(1, 1, 0));
				var point4 = Point(new Vector3(1, 0, 0));

				Shader.SetGlobalVector("Point1", point1);
				Shader.SetGlobalVector("Point2", point2);
				Shader.SetGlobalVector("Point3", point3);
				Shader.SetGlobalVector("Point4", point4);

				Blitter.BlitTexture(commandBuffer, RenderTexture, new Vector4(1, 1, 0, 0), Settings.FrustrumMaterial, ShaderPass.Frustrum);

				Vector2 Point(Vector3 viewportPoint)
				{
					var ray = camera.ViewportPointToRay(viewportPoint, UnityEngine.Camera.MonoOrStereoscopicEye.Mono);
					plane.Raycast(ray, out float enter);
					var worldSpacePoint = ray.origin + ray.direction * enter;
					//Debug.DrawRay(point1, Vector3.up * 100, Color.red);
					var orhoPoint = new Vector2((worldSpacePoint.x / (MapSettings.Size.x / 2) + 1) * 0.5f, (worldSpacePoint.z / (MapSettings.Size.y / 2) + 1) * 0.5f);
					return orhoPoint;
				}
			}
		}
	}
}
