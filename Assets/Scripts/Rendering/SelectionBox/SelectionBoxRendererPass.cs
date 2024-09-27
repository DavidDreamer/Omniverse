using System;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionBoxRendererPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int SelectionBox { get; } = Shader.PropertyToID(nameof(SelectionBox));
		}

		private SelectionBoxRenderer Renderer { get; }

		public SelectionBoxRendererPass(SelectionBoxRenderer renderer)
		{
			Renderer = renderer;
		}

		public void Dispose()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, "SelectionBox");
			var commandBuffer = scope.CommandBuffer;

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

			RTHandle cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
			commandBuffer.Blit(cameraColorTargetHandle, cameraColorTargetHandle, config.Material);
		}
	}
}