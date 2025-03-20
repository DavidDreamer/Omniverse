using System;
using Dreambox.Core;
using Dreambox.Rendering.Core;
using Omniverse.Input;
using Unity.Entities;
using Unity.Transforms;
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

			Matrices = new Matrix4x4[Selection.Capacity];
			Colors = new Vector4[Selection.Capacity];
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

			Player player = ECSUtils.GetSingleton<Player>();
			Selection selection = ECSUtils.GetSingleton<Selection>();

			MaterialPropertyBlock.Clear();

			int i = 0;
			foreach (Entity entity in selection.Entities)
			{
				EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
				var localToWorld = entityManager.GetComponentData<LocalToWorld>(entity);
				var faction = entityManager.GetSharedComponent<Faction>(entity);
				Matrices[i] = (Matrix4x4)localToWorld.Value * MatrixUtils.WorldUpRotation * Matrix4x4.Translate(Vector3.up * 0.01f);
				Colors[i] = player.FactionID == faction.ID ?
					selection.Entity == entity ? config.MainSelectionColor : config.AllyColor :
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
				selection.Entities.Length,
				MaterialPropertyBlock);
		}
	}
}
