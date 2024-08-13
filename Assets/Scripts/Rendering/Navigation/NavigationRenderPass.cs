using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRenderPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID(nameof(Lifetime));
		}

		private NavigationRenderer Renderer { get; }

		private Matrix4x4[] Matrices { get; }

		private float[] Lifetimes { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public NavigationRenderPass(NavigationRenderer renderer)
		{
			Renderer = renderer;

			Matrices = new Matrix4x4[Renderer.Config.Capacity];
			Lifetimes = new float[Renderer.Config.Capacity];
			MaterialPropertyBlock = new();
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, nameof(NavigationRenderPass));
			var commandBuffer = scope.CommandBuffer;


			NavigationRenderConfig config = Renderer.Config;
			DrawMeshParams drawMeshParams = config.DrawMeshParams;

			float time = Time.time;

			int i = 0;
			foreach (NavigationPoint navigationPoint in Renderer.NavigationPoints)
			{
				Matrices[i] = Matrix4x4.TRS(navigationPoint.Position, Quaternion.identity, Vector3.one);
				Lifetimes[i] = Mathf.Clamp01((time - navigationPoint.Time) / config.Lifetime);
				i++;
			}

			MaterialPropertyBlock.SetFloatArray(ShaderVariables.Lifetime, Lifetimes);

			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				Renderer.NavigationPoints.Count,
				MaterialPropertyBlock);
		}
	}
}
