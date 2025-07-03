using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

namespace Omniverse.Rendering
{
	public class BuilderRenderPass : ScriptableRenderPass
	{
		private BuilderRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		public BuildingDesc BuildingDesc { get; set; }

		public Pointer Pointer { get; set; }

		private class PassData
		{
			public BuilderRenderSettings Settings;
			public EntityManager EntityManager;
			public BuildingDesc BuildingDesc;
			public Pointer Pointer;
		}

		public BuilderRenderPass(BuilderRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Selection Box", out var data))
			{
				data.EntityManager = EntityManager;
				data.Settings = Settings;
				data.Pointer = Pointer;
				data.BuildingDesc = BuildingDesc;

				builder.AllowGlobalStateModification(true);

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					Mesh mesh = data.BuildingDesc.Mesh;
					Material material = data.Settings.Material;

					Vector3 boundsSize = mesh.bounds.size;
					commandBuffer.SetGlobalVector("BoundsSize", boundsSize);

					float3 position = data.Pointer.CellPosiiton;
					var matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
					commandBuffer.DrawMesh(mesh, matrix, material, 0, 0);
				});
			}
		}
	}
}
