using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionBoxRendererPass : ScriptableRenderPass
	{
		private class PassData
		{
			public Material Material;
		}

		private Material Material { get; }

		public SelectionBoxRendererPass(Material material)
		{
			Material = material;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Selection Box", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				data.Material = Material;
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(data, context));
			}
		}

		private static void Execute(PassData data, RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;
			CoreUtils.DrawFullScreen(commandBuffer, data.Material);
		}
	}
}