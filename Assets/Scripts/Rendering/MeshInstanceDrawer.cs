using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Omniverse.Rendering
{
	public class MeshInstanceDrawer
	{
		private MeshDrawSettings Settings { get; }

		public int BatchSize { get; }

		private Matrix4x4[] Matrices { get; }

		protected int Count { get; set; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public MeshInstanceDrawer(MeshDrawSettings settings, int batchSize)
		{
			Settings = settings;
			BatchSize = batchSize;

			Matrices = new Matrix4x4[BatchSize];

			MaterialPropertyBlock = new();
		}

		public void AddInstance(Matrix4x4 matrix)
		{
			Matrices[Count] = matrix;
			Count++;
		}

		public void DrawBatch(CommandBuffer commandBuffer)
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

		public void DrawBatch(RasterCommandBuffer commandBuffer)
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

		protected virtual void SetupBatch(MaterialPropertyBlock materialPropertyBlock)
		{
		}
	}
}
