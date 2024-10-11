using System;
using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class FogOfWarPass : ScriptableRenderPass, IDisposable
	{
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

		public override void Execute(
			ScriptableRenderContext context,
			ref RenderingData renderingData)
		{
			using var scope = new CommandBufferContextScope(context, "FogOfWar.Apply");
			CommandBuffer commandBuffer = scope.CommandBuffer;

			CoreUtils.DrawFullScreen(commandBuffer, Material, shaderPassId: 0);
		}
	}
}
