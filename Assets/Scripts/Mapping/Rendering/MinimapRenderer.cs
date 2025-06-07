using Dreambox.Rendering.Core;
using Omniverse.Rendering;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Omniverse.Mapping
{
	public class MinimapRenderer : RenderFeature
	{
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

		public DrawMeshParams DrawMeshParams;

		public Material FrustrumMaterial;

		public RenderTexture RenderTexture { get; private set; }

		private ConstantComputeBuffer<MapShaderProperties> PropertiesBuffer { get; set; }

		private VirtualCamera Camera { get; set; }

		private MinimapUnitDrawer MinimapUnitDrawer { get; set; }

		public void OnEnable()
		{
			RenderPipelineManager.beginContextRendering += OnBeginContextRendering;

			var mapSettings = EntityManager.GetSingleton<MapSettings>();

			RenderTexture = new RenderTexture(
				mapSettings.Size.x,
				mapSettings.Size.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(mapSettings.Size.x, mapSettings.Size.y, 0, 0)
			};

			PropertiesBuffer = new ConstantComputeBuffer<MapShaderProperties>(ShaderVariables.MapProperties);
			PropertiesBuffer.SetData(mapShaderProperties);

			Camera = new VirtualCamera(mapSettings.Size.x / 2);

			MinimapUnitDrawer = new(DrawMeshParams, 64);
		}

		private void OnBeginContextRendering(ScriptableRenderContext context, System.Collections.Generic.List<Camera> arg2)
		{
			using var scope = new CommandBufferContextScope(context, "Minimap");

			CommandBuffer commandBuffer = scope.CommandBuffer;

			commandBuffer.SetViewProjectionMatrices(Camera.WorldToCameraMatrix, Camera.ProjectionMatrix);

			commandBuffer.SetRenderTarget(RenderTexture);
			commandBuffer.ClearRenderTarget(true, true, Color.clear);

			DrawUnits();
			DrawFrustrum();

			void DrawUnits()
			{
				var player = EntityManager.GetSingleton<Player>();
				var query = new EntityQueryBuilder(Allocator.Temp).WithAspect<Unit>();
				var entities = EntityManager.CreateEntityQuery(query).ToEntityArray(Allocator.Temp);

				int drawnCount = 0;

				while (drawnCount < entities.Length)
				{
					int currentBatchSize = math.min(entities.Length - drawnCount, MinimapUnitDrawer.BatchSize);

					for (int i = 0; i < currentBatchSize; i++)
					{
						Entity entity = entities[i];

						var localTransform = EntityManager.GetComponentData<LocalTransform>(entity);
						float4x4 matrix = localTransform.ToMatrix();
						var faction = EntityManager.GetComponentData<Faction>(entity);
						Color tint = player.FactionID == faction.ID ? Color.green : Color.red;
						MinimapUnitDrawer.AddInstance(matrix, tint);
					}

					MinimapUnitDrawer.DrawBatch(commandBuffer);

					drawnCount += currentBatchSize;
				}
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

				Blitter.BlitTexture(commandBuffer, RenderTexture, new Vector4(1, 1, 0, 0), FrustrumMaterial, 0);

				Vector2 Point(Vector3 viewportPoint)
				{
					var ray = camera.ViewportPointToRay(viewportPoint, UnityEngine.Camera.MonoOrStereoscopicEye.Mono);
					plane.Raycast(ray, out float enter);
					var worldSpacePoint = ray.origin + ray.direction * enter;
					//Debug.DrawRay(point1, Vector3.up * 100, Color.red);
					var orhoPoint = new Vector2((worldSpacePoint.x / 32 + 1) * 0.5f, (worldSpacePoint.z / 32 + 1) * 0.5f);
					return orhoPoint;
				}
			}
		}

		public void OnDisable()
		{
			RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;

			Destroy(RenderTexture);

			PropertiesBuffer.Dispose();
		}
	}
}
