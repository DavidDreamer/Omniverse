using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class NavigationPointDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID(nameof(Lifetime));
		}

		private float[] Lifetimes { get; }

		public NavigationPointDrawer(MeshDrawSettings settings, int batchSize) : base(settings, batchSize)
		{
			Lifetimes = new float[BatchSize];
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, float lifetime)
		{
			Lifetimes[Count] = lifetime;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetFloatArray(ShaderVariables.Lifetime, Lifetimes);
		}
	}
}
