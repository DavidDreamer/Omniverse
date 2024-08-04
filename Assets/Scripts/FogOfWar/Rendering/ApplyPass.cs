using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.FogOfWar.Rendering
{
	public class ApplyPass : ScriptableRenderPass
	{
		private Manager Manager { get; }

		private Material Material { get; }

		public ApplyPass(Manager manager, Shader shader)
		{
			Manager = manager;
			Material = new Material(shader);
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
