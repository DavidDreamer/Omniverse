using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.FogOfWar.Rendering
{
	public class ApplyPass: ScriptableRenderPass
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
			CommandBuffer cmd = CommandBufferPool.Get("FogOfWar.Apply");

			RTHandle cameraTargetHandle =
				renderingData.cameraData.renderer.cameraColorTargetHandle;

			Blitter.BlitCameraTexture(cmd, cameraTargetHandle, cameraTargetHandle, Material, 0);

			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}
	}
}
