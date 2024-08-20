using System;
using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRenderPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID(nameof(Lifetime));
		}

		private NavigationRenderer RendererFeature { get; }

		private NavigationRendererConfig Config { get; }

		private Matrix4x4[] Matrices { get; }

		private float[] Lifetimes { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public NavigationRenderPass(NavigationRenderer rendererFeature)
		{
			RendererFeature = rendererFeature;
			Config = rendererFeature.Config;

			Matrices = new Matrix4x4[rendererFeature.Config.Capacity];
			Lifetimes = new float[rendererFeature.Config.Capacity];
			MaterialPropertyBlock = new();
		}

		public void Dispose()
		{
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using CommandBufferContextScope scope = new(context, nameof(NavigationRenderPass));
			var commandBuffer = scope.CommandBuffer;

			DrawMeshParams drawMeshParams = Config.DrawMeshParams;

			float time = Time.time;

			int i = 0;
			foreach (NavigationPoint navigationPoint in RendererFeature.Points)
			{
				Matrices[i] = Matrix4x4.TRS(navigationPoint.Position, Quaternion.identity, Vector3.one);
				Lifetimes[i] = Mathf.Clamp01((time - navigationPoint.Time) / Config.Lifetime);
				i++;
			}

			MaterialPropertyBlock.SetFloatArray(ShaderVariables.Lifetime, Lifetimes);

			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				RendererFeature.Points.Count,
				MaterialPropertyBlock);
		}
	}
}
