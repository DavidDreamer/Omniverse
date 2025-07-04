using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class SelectionDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID($"_{nameof(BaseColor)}");
			public static int Radius { get; } = Shader.PropertyToID($"_{nameof(Radius)}");
		}

		private Vector4[] BaseColor { get; }

		private float[] Radius { get; }

		public SelectionDrawer(MeshDrawSettings settings, RenderingLayerMask renderingLayerMask, int batchSize) : base(settings, batchSize)
		{
			BaseColor = new Vector4[BatchSize];
			Radius = new float[BatchSize];

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

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, Color baseColor, float radius)
		{
			BaseColor[Count] = baseColor;
			Radius[Count] = radius;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColor);
			materialPropertyBlock.SetFloatArray(ShaderVariables.Radius, Radius);
		}
	}
}
