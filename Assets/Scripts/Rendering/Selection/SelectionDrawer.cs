using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class SelectionDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));
		}

		private Vector4[] BaseColors { get; }

		public SelectionDrawer(MeshDrawSettings settings, int batchSize) : base(settings, batchSize)
		{
			BaseColors = new Vector4[BatchSize];
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, Color baseColor)
		{
			BaseColors[Count] = baseColor;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColors);
		}
	}
}
