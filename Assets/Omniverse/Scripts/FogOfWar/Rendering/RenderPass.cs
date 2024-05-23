using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.FogOfWar.Rendering
{
	public class RenderPass: ScriptableRenderPass
	{
		private Shaders Shaders { get; }
		
		private Material ApplyMaterial;

		private RenderTextureDescriptor textureDescriptor;

		private RTHandle textureHandle;

		public RenderPass(Shaders shaders)
		{
			Shaders = shaders;
			
			ApplyMaterial = new Material(Shaders.Apply);

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

			Blitter.BlitCameraTexture(cmd, cameraTargetHandle, cameraTargetHandle, ApplyMaterial, 0);

			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}
		
		public void Dispose()
		{
			CoreUtils.Destroy(ApplyMaterial);

			if (textureHandle != null) textureHandle.Release();
		}
	}
}
