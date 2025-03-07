using System;
using Dreambox.Core;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionRendererPass : ScriptableRenderPass, IDisposable
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));
		}

		private class PassData
		{
		}

		private SelectionRenderer Renderer { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] Colors { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public SelectionRendererPass(SelectionRenderer renderer)
		{
			Renderer = renderer;

			Matrices = new Matrix4x4[Selector.Capacity];
			Colors = new Vector4[Selector.Capacity];
			MaterialPropertyBlock = new MaterialPropertyBlock();
		}

		public void Dispose()
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Selection", out var data))
			{
				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc((PassData data, RasterGraphContext context) => Execute(context));
			}
		}

		private void Execute(RasterGraphContext context)
		{
			RasterCommandBuffer commandBuffer = context.cmd;

			SelectionRendererConfig config = Renderer.Config;
			Selector selector = Renderer.Selector;

			MaterialPropertyBlock.Clear();

			int i = 0;
			foreach (UnitObsolete unit in selector.SelectedUnits)
			{
				Matrices[i] = unit.transform.localToWorldMatrix * MatrixUtils.WorldUpRotation;
				Colors[i] = Renderer.Player.FactionID == unit.FactionID ?
					selector.SelectedUnit == unit ? config.MainSelectionColor : config.AllyColor :
					config.EnemyColor;
				i++;
			}

			MaterialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, Colors);

			DrawMeshParams drawMeshParams = config.DrawMeshParams;
			commandBuffer.DrawMeshInstanced(
				drawMeshParams.Mesh,
				drawMeshParams.SubmeshIndex,
				drawMeshParams.Material,
				drawMeshParams.ShaderPass,
				Matrices,
				selector.SelectedUnits.Count,
				MaterialPropertyBlock);
		}
	}
}
