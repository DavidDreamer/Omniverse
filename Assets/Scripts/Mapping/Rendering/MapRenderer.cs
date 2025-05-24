using Dreambox.Rendering.Core;
using Omniverse.Rendering;
using Unity.Collections;
using Unity.Entities;
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
