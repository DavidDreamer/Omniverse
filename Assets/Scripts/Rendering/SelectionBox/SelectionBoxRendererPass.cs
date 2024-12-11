using System;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionBoxRendererPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int SelectionBox { get; } = Shader.PropertyToID(nameof(SelectionBox));
		}

		private class PassData
		{
		}

		private SelectionBoxRenderer Renderer { get; }

		public SelectionBoxRendererPass(SelectionBoxRenderer renderer)
		{
			Renderer = renderer;
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Selection Box", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.AllowGlobalStateModification(true);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;

			SelectionBoxRendererConfig config = Renderer.Config;
			Selector selector = Renderer.Selector;

			float x, z, y, w;

			if (selector.StartPosition.x > selector.EndPosition.x)
			{
				x = selector.EndPosition.x;
				z = selector.StartPosition.x;
			}
			else
			{
				x = selector.StartPosition.x;
				z = selector.EndPosition.x;
			}

			if (selector.StartPosition.y > selector.EndPosition.y)
			{
				y = selector.EndPosition.y;
				w = selector.StartPosition.y;
			}
			else
			{
				y = selector.StartPosition.y;
				w = selector.EndPosition.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			commandBuffer.SetGlobalVector(ShaderVariables.SelectionBox, selectionBox);

			CoreUtils.DrawFullScreen(commandBuffer, config.Material);
		}
	}
}