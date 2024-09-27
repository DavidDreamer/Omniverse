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
			UnitSelector unitSelector = Renderer.UnitSelector;

			float x, z, y, w;

			if (unitSelector.StartPosition.x > unitSelector.EndPosition.x)
			{
				x = unitSelector.EndPosition.x;
				z = unitSelector.StartPosition.x;
			}
			else
			{
				x = unitSelector.StartPosition.x;
				z = unitSelector.EndPosition.x;
			}

			if (unitSelector.StartPosition.y > unitSelector.EndPosition.y)
			{
				y = unitSelector.EndPosition.y;
				w = unitSelector.StartPosition.y;
			}
			else
			{
				y = unitSelector.StartPosition.y;
				w = unitSelector.EndPosition.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			commandBuffer.SetGlobalVector(ShaderVariables.SelectionBox, selectionBox);

			RTHandle cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
			commandBuffer.Blit(cameraColorTargetHandle, cameraColorTargetHandle, config.Material);
		}
	}
}