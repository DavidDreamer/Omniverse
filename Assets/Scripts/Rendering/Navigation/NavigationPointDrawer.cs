using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class NavigationPointDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID($"_{nameof(Lifetime)}");
		}

		private float[] Lifetime { get; }

		public NavigationPointDrawer(MeshDrawSettings settings, RenderingLayerMask renderingLayerMask, int batchSize) : base(settings, batchSize)
		{
			Lifetime = new float[BatchSize];

			float[] decalLayerMaskFromDecal = new float[BatchSize];
			for (int i = 0; i < BatchSize; i++)
			{
				decalLayerMaskFromDecal[i] = renderingLayerMask.value;
			}
			MaterialPropertyBlock.SetFloatArray(DecalShaderVariables.DecalLayerMaskFromDecal, decalLayerMaskFromDecal);

			var matrix = new Matrix4x4(new Vector4(1, 0, 0, 1), new Vector4(0, 0, 1, 1), new Vector4(0, 1, 0, 0), new Vector4(1, 0, 0, 0));
			Matrix4x4[] normalToWorld = new Matrix4x4[BatchSize];
			for (int i = 0; i < BatchSize; i++)
			{
				normalToWorld[i] = matrix;
			}
			MaterialPropertyBlock.SetMatrixArray(DecalShaderVariables.NormalToWorld, normalToWorld);
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, float lifetime)
		{
			Lifetime[Count] = lifetime;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetFloatArray(ShaderVariables.Lifetime, Lifetime);
		}
	}
}
