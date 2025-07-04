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
			public static int Radius { get; } = Shader.PropertyToID(nameof(Radius));
		}

		private Vector4[] BaseColors { get; }

		private float[] Radiuses { get; }

		public SelectionDrawer(MeshDrawSettings settings, int batchSize) : base(settings, batchSize)
		{
			BaseColors = new Vector4[BatchSize];
			Radiuses = new float[BatchSize];
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, Color baseColor, float radius)
		{
			BaseColors[Count] = baseColor;
			Radiuses[Count] = radius;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColors);
			materialPropertyBlock.SetFloatArray(ShaderVariables.Radius, Radiuses);
		}
	}
}
