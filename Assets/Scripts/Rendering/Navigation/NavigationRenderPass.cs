using System.Collections.Generic;
using Dreambox.Rendering.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRenderPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int Lifetime { get; } = Shader.PropertyToID(nameof(Lifetime));
		}

		private class PassData
		{
		}

		private NavigationRenderSettings Config { get; set; }

		private Queue<NavigationPoint> Points { get; }

		private Matrix4x4[] Matrices { get; }

		private float[] Lifetimes { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public NavigationRenderPass(NavigationRenderSettings config, Queue<NavigationPoint> poitns)
		{
			Config = config;
			Points = poitns;
			Matrices = new Matrix4x4[config.Capacity];
			Lifetimes = new float[config.Capacity];
			MaterialPropertyBlock = new();
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

			double time = Time.time;
			int i = 0;
			foreach (NavigationPoint point in Points)
			{
				Matrices[i] = point.Matrix;
				Lifetimes[i] = (float)math.clamp((time - point.Time) / (double)Config.Lifetime, 0, 1);
				i++;
			}

			MeshDrawSettings settings = Config.MeshDrawSettings;

			commandBuffer.DrawMeshInstanced(
				settings.Mesh,
				settings.SubmeshIndex,
				settings.Material,
				settings.ShaderPass,
				Matrices,
				Points.Count,
				MaterialPropertyBlock);
		}
	}
}
