using Dreambox.Rendering.Core;
using Omniverse.Rendering;
using UnityEngine;

namespace Omniverse.Mapping
{
	public class MinimapUnitDrawer : MeshInstanceDrawer
	{
		private static class ShaderVariables
		{
			public static int Tint { get; } = Shader.PropertyToID(nameof(Tint));
		}

		private Vector4[] Tints { get; }

		public MinimapUnitDrawer(MeshDrawSettings settings, int batchSize) : base(settings, batchSize)
		{
			Tints = new Vector4[batchSize];
		}

		public void AddInstance(Matrix4x4 matrix, Color tint)
		{
			Tints[Count] = tint;
			AddInstance(matrix);
		}

		protected override void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
			materialPropertyBlock.SetVectorArray(ShaderVariables.Tint, Tints);
		}
	}
}
