using Omniverse.Input;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class BuilderRenderPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int BuilderBoundsSize { get; } = Shader.PropertyToID($"_{nameof(BuilderBoundsSize)}");
			public static int BuilderFocusPoint { get; } = Shader.PropertyToID($"_{nameof(BuilderFocusPoint)}");
		}

		private BuilderRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		public Entity Building { get; set; }

		public Pointer Pointer { get; set; }

		private class PassData
		{
			public BuilderRenderSettings Settings;
			public EntityManager EntityManager;
			public Entity Building;
			public Pointer Pointer;
		}

		public BuilderRenderPass(BuilderRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Builder", out var data))
			{
				data.EntityManager = EntityManager;
				data.Settings = Settings;
				data.Pointer = Pointer;
				data.Building = Building;

				builder.AllowGlobalStateModification(true);

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					var materialMeshInfo = data.EntityManager.GetComponentData<MaterialMeshInfo>(data.Building);
					var renderMeshArray = data.EntityManager.GetSharedComponentManaged<RenderMeshArray>(data.Building);
					Mesh mesh = renderMeshArray.GetMesh(materialMeshInfo);

					Material material = data.Settings.Material;

					Vector3 boundsSize = mesh.bounds.size;
					commandBuffer.SetGlobalVector(ShaderVariables.BuilderBoundsSize, boundsSize);

					Vector3 focusPoint = (Vector3)data.Pointer.CellPosiiton;
					commandBuffer.SetGlobalVector(ShaderVariables.BuilderFocusPoint, focusPoint);

					CoreUtils.DrawFullScreen(commandBuffer, data.Settings.GridMaterial, shaderPassId: 0);

					float3 position = data.Pointer.CellPosiiton;
					var matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
					commandBuffer.DrawMesh(mesh, matrix, material, 0, 0);
				});
			}
		}
	}
}
