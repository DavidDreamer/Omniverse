using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public static class DecalShaderVariables
	{
		public static int NormalToWorld { get; } = Shader.PropertyToID($"_{nameof(NormalToWorld)}");
		public static int DecalLayerMaskFromDecal { get; } = Shader.PropertyToID($"_{nameof(DecalLayerMaskFromDecal)}");
	}

	public class MeshInstanceDrawer
	{
		private MeshDrawSettings Settings { get; }

		public int BatchSize { get; }

		private Matrix4x4[] Matrices { get; }

		protected int Count { get; set; }

		protected MaterialPropertyBlock MaterialPropertyBlock { get; }

		public MeshInstanceDrawer(MeshDrawSettings settings, int batchSize)
		{
			Settings = settings;
			BatchSize = batchSize;

			Matrices = new Matrix4x4[BatchSize];

			MaterialPropertyBlock = new();
		}

		public void Draw(CommandBuffer commandBuffer, Matrix4x4 matrix)
		{
			Matrices[Count] = matrix;
			Count++;

			if (Count == BatchSize)
			{
				DrawBatch(commandBuffer);
			}
		}

		public void Draw(RasterCommandBuffer commandBuffer, Matrix4x4 matrix)
		{
			Matrices[Count] = matrix;
			Count++;

			if (Count == BatchSize)
			{
				DrawBatch(commandBuffer);
			}
		}

		private void DrawBatch(CommandBuffer commandBuffer)
		{
			SetupBatch(MaterialPropertyBlock);

			commandBuffer.DrawMeshInstanced(
				Settings.Mesh,
				Settings.SubmeshIndex,
				Settings.Material,
				Settings.ShaderPass,
				Matrices,
				Count,
				MaterialPropertyBlock);

			Count = 0;
		}

		private void DrawBatch(RasterCommandBuffer commandBuffer)
		{
			SetupBatch(MaterialPropertyBlock);

			commandBuffer.DrawMeshInstanced(
				Settings.Mesh,
				Settings.SubmeshIndex,
				Settings.Material,
				Settings.ShaderPass,
				Matrices,
				Count,
				MaterialPropertyBlock);

			Count = 0;
		}

		public void Flush(CommandBuffer commandBuffer)
		{
			if (Count == 0)
			{
				return;
			}

			DrawBatch(commandBuffer);
		}

		public void Flush(RasterCommandBuffer commandBuffer)
		{
			if (Count == 0)
			{
				return;
			}

			DrawBatch(commandBuffer);
		}

		protected virtual void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
		}
	}
}
