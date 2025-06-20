using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class FogOfWarPass : ScriptableRenderPass
	{
		private class PassData
		{
			public Material Material;
		}

		private Material Material { get; }

		public FogOfWarPass(Material material)
		{
			Material = material;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("FogOfWar.Apply", out var data))
			{
				data.Material = Material;

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;
					CoreUtils.DrawFullScreen(commandBuffer, data.Material, shaderPassId: FogOfWarRenderSystem.ShaderPass.Apply);
				});
			}
		}
	}
}
