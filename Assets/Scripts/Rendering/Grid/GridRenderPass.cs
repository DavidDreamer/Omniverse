using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class GridRenderPass : ScriptableRenderPass
	{
		private GridRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private class PassData
		{
			public GridRenderSettings Settings;
			public EntityManager EntityManager;
		}

		public GridRenderPass(GridRenderSettings settings, EntityManager entityManager)
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

				builder.AllowGlobalStateModification(true);

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;
					CoreUtils.DrawFullScreen(commandBuffer, data.Settings.Material, shaderPassId: 0);
				});
			}
		}
	}
}
