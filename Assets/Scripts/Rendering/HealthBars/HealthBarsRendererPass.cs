using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class HealthBarsRendererPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));

			public static int SecondColor { get; } = Shader.PropertyToID(nameof(SecondColor));

			public static int Amount { get; } = Shader.PropertyToID(nameof(Amount));
		}

		private class PassData
		{
		}

		private HealthBarsRenderer Renderer { get; }

		private HealthBarsRendererConfig Config { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] BaseColors { get; }

		private Vector4[] SecondColors { get; }

		private float[] Amounts { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public HealthBarsRendererPass(HealthBarsRenderer renderer)
		{
			Renderer = renderer;
			Config = renderer.Config;

			Matrices = new Matrix4x4[Config.MaxCount];
			BaseColors = new Vector4[Config.MaxCount];
			SecondColors = new Vector4[Config.MaxCount];
			Amounts = new float[Config.MaxCount];

			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Health Bars", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;

			UnitManager unitManager = Renderer.UnitManager;
			var units = unitManager.Units;

			MaterialPropertyBlock.Clear();

			int count = Mathf.Min(units.Count, Config.MaxCount);

			if (count == 0)
			{
				return;
			}

			for (int i = 0; i < count; i++)
			{
				UnitObsolete unit = units[i];

				var matrix = unit.transform.localToWorldMatrix * Matrix4x4.Translate(Config.Offset);
				Matrices[i] = matrix;

				HealthBarColors colors = Renderer.Player.FactionID == unit.FactionID ? Config.AllyColors : Config.EnemyColors;
				BaseColors[i] = colors.BaseColor;
				SecondColors[i] = colors.SecondColor;

				var healthProperty = unit.Properties[PropertyID.Health];
				Amounts[i] = healthProperty.Amount / healthProperty.Desc.Range.Max;
			}

			MaterialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, BaseColors);
			MaterialPropertyBlock.SetVectorArray(ShaderVariables.SecondColor, SecondColors);
			MaterialPropertyBlock.SetFloatArray(ShaderVariables.Amount, Amounts);

			var drawMeshParams = Config.DrawMeshParams;
			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				count,
				MaterialPropertyBlock);
		}
	}
}
