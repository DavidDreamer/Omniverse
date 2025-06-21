using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class HealthBarDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));

			public static int SecondColor { get; } = Shader.PropertyToID(nameof(SecondColor));

			public static int Amount { get; } = Shader.PropertyToID(nameof(Amount));
		}

		private Vector4[] BaseColors { get; }

		private Vector4[] SecondColors { get; }

		private float[] Amounts { get; }

		public HealthBarDrawer(MeshDrawSettings settings, int batchSize) : base(settings, batchSize)
		{
			BaseColors = new Vector4[BatchSize];
			SecondColors = new Vector4[BatchSize];
			Amounts = new float[BatchSize];
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix, Color baseColor, Color secondColor, float amount)
		{
			BaseColors[Count] = baseColor;
			SecondColors[Count] = secondColor;
			Amounts[Count] = amount;

			Draw(commandBuffer, matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColors);
			materialPropertyBlock.SetVectorArray(ShaderVariables.SecondColor, SecondColors);
			materialPropertyBlock.SetFloatArray(ShaderVariables.Amount, Amounts);
		}
	}
}
