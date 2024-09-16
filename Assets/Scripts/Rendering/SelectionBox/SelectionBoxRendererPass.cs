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

			if (unitSelector.SelectionBoxStart.x > unitSelector.SelectionBoxEnd.x)
			{
				x = unitSelector.SelectionBoxEnd.x;
				z = unitSelector.SelectionBoxStart.x;
			}
			else
			{
				x = unitSelector.SelectionBoxStart.x;
				z = unitSelector.SelectionBoxEnd.x;
			}

			if (unitSelector.SelectionBoxStart.y > unitSelector.SelectionBoxEnd.y)
			{
				y = unitSelector.SelectionBoxEnd.y;
				w = unitSelector.SelectionBoxStart.y;
			}
			else
			{
				y = unitSelector.SelectionBoxStart.y;
				w = unitSelector.SelectionBoxEnd.y;
			}

			Vector4 selectionBox = new(x, y, z, w);
			commandBuffer.SetGlobalVector(ShaderVariables.SelectionBox, selectionBox);

			RTHandle cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
			commandBuffer.Blit(cameraColorTargetHandle, cameraColorTargetHandle, config.Material);
		}
	}
}