using System;
using System.Collections.Generic;
using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRenderPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID(nameof(Lifetime));
		}

		private class PassData
		{
		}

		private NavigationRendererConfig Config { get; set; }

		private Queue<NavigationPoint> Points { get; }

		private Matrix4x4[] Matrices { get; }

		private float[] Lifetimes { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public NavigationRenderPass(NavigationRendererConfig config, Queue<NavigationPoint> poitns)
		{
			Config = config;
			Points = poitns;
			Matrices = new Matrix4x4[config.Capacity];
			Lifetimes = new float[config.Capacity];
			MaterialPropertyBlock = new();
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Navigation", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;

			MaterialPropertyBlock.SetFloatArray(ShaderVariables.Lifetime, Lifetimes);

			float time = Time.time;
			int i = 0;
			foreach (NavigationPoint point in Points)
			{
				Matrices[i] = point.Matrix;
				Lifetimes[i] = Mathf.Clamp01((time - point.Time) / Config.Lifetime);
				i++;
			}

			MeshDrawSettings drawMeshParams = Config.DrawMeshParams;

			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				Points.Count,
				MaterialPropertyBlock);
		}
	}
}
