using Dreambox.Rendering.Core;
using Omniverse.Rendering;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Omniverse.Mapping
{
	public class MapRenderer : RenderFeature
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

		public Mesh mesh;

		public Material material;

		public Material FrustrumMaterial;

		public RenderTexture RenderTexture { get; private set; }

		private ConstantComputeBuffer<MapShaderProperties> PropertiesBuffer { get; set; }

		private VirtualCamera Camera { get; set; }

		public void OnEnable()
		{
			RenderPipelineManager.beginContextRendering += OnBeginContextRendering;

			var gameOptions = EntityManager.GetSingletonManaged<GameOptions>();

			RenderTexture = new RenderTexture(
				gameOptions.MapSize.x,
				gameOptions.MapSize.y,
				GraphicsFormat.R16G16B16A16_SFloat,
				GraphicsFormat.D16_UNorm)
			{
				name = "Minimap"
			};

			var mapShaderProperties = new MapShaderProperties
			{
				MapSize = new Vector4(gameOptions.MapSize.x, gameOptions.MapSize.y, 0, 0)
			};

			PropertiesBuffer = new ConstantComputeBuffer<MapShaderProperties>(ShaderVariables.MapProperties);
			PropertiesBuffer.SetData(mapShaderProperties);

			Camera = new VirtualCamera(gameOptions.MapSize.x / 2);
		}

		private void OnBeginContextRendering(ScriptableRenderContext context, System.Collections.Generic.List<Camera> arg2)
		{
			using (var scope = new CommandBufferContextScope(context, "TODO"))
			{
				CommandBuffer commandBuffer = scope.CommandBuffer;
				
				commandBuffer.SetViewProjectionMatrices(Camera.WorldToCameraMatrix, Camera.ProjectionMatrix);

				commandBuffer.SetRenderTarget(RenderTexture);
				commandBuffer.ClearRenderTarget(true, true, Color.clear);

				var query = new EntityQueryBuilder(Allocator.Temp).WithAspect<Unit>();
				var entities = EntityManager.CreateEntityQuery(query).ToEntityArray(Allocator.Temp);

				foreach (var entity in entities)
				{
					var localTransform = EntityManager.GetComponentData<LocalTransform>(entity);
					commandBuffer.DrawMesh(mesh, localTransform.ToMatrix(), material, 0, 0);
				}

				var camera = UnityEngine.Camera.main;
				var plane = new Plane(Vector3.up, 0);

				var point1 = Point(new Vector3(0, 0, 0));
				var point2 = Point(new Vector3(0, 1, 0));
				var point3 = Point(new Vector3(1, 1, 0));
				var point4 = Point(new Vector3(1, 0, 0));

				//Debug.DrawLine(point1, point2, Color.red);
				//Debug.DrawLine(point2, point3, Color.red);
				//Debug.DrawLine(point3, point4, Color.red);
				//Debug.DrawLine(point4, point1, Color.red);

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
