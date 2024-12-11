using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class FogOfWarPass : ScriptableRenderPass, IDisposable
	{
		private class PassData
		{
		}

		private FogOfWarRenderer Renderer { get; }

		private FogOfWarRendererConfig Config { get; }

		private Material Material { get; }

		public FogOfWarPass(FogOfWarRenderer renderer)
		{
			Renderer = renderer;
			Config = renderer.Config;
			Material = new Material(Config.ApplyShader);
		}

		public void Dispose()
		{
			CoreUtils.Destroy(Material);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("FogOfWar.Apply", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;
			CoreUtils.DrawFullScreen(commandBuffer, Material, shaderPassId: 0);
		}
	}
}
