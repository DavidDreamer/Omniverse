using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Visibility.Rendering
{
	public class FogOfWarPass: ScriptableRenderPass
	{
		private Material material;

		private RenderTextureDescriptor textureDescriptor;

		private RTHandle textureHandle;

		public FogOfWarPass(Material material)
		{
			this.material = material;

			textureDescriptor = new RenderTextureDescriptor(Screen.width,
				Screen.height, RenderTextureFormat.Default, 0);
		}

		public override void Configure(
			CommandBuffer cmd,
			RenderTextureDescriptor cameraTextureDescriptor)
		{
			// Set the texture size to be the same as the camera target size.
			textureDescriptor.width = cameraTextureDescriptor.width;
			textureDescriptor.height = cameraTextureDescriptor.height;

			// Check if the descriptor has changed, and reallocate the RTHandle if necessary
			RenderingUtils.ReAllocateIfNeeded(ref textureHandle, textureDescriptor);
		}

		public override void Execute(
			ScriptableRenderContext context,
			ref RenderingData renderingData)
		{
			CommandBuffer cmd = CommandBufferPool.Get("FogOfWar");

			RTHandle cameraTargetHandle =
				renderingData.cameraData.renderer.cameraColorTargetHandle;

			Blitter.BlitCameraTexture(cmd, cameraTargetHandle, cameraTargetHandle, material, 0);

			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}
		
		public void Dispose()
		{
			CoreUtils.Destroy(material);

			if (textureHandle != null) textureHandle.Release();
		}
	}
}
