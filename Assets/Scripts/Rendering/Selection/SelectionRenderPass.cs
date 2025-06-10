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
	public class SelectionRenderPass : ScriptableRenderPass
	{
		private static class ShaderVariables
		{
			public static int BaseColor { get; } = Shader.PropertyToID(nameof(BaseColor));
		}

		private class PassData
		{
		}

		public Player Player { get; set; }

		public Selection Selection { get; set; }

		private SelectionRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private Matrix4x4[] Matrices { get; }

		private Vector4[] Colors { get; }

		private MaterialPropertyBlock MaterialPropertyBlock { get; }

		public SelectionRenderPass(SelectionRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;

			Matrices = new Matrix4x4[Selection.Capacity];
			Colors = new Vector4[Selection.Capacity];
			MaterialPropertyBlock = new MaterialPropertyBlock();
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

			MaterialPropertyBlock.Clear();

			int i = 0;
			foreach (Entity entity in Selection.Entities)
			{
				var localToWorld = EntityManager.GetComponentData<LocalToWorld>(entity);
				var faction = EntityManager.GetComponentData<Faction>(entity);
				Matrices[i] = (Matrix4x4)localToWorld.Value * MatrixUtils.WorldUpRotation * Matrix4x4.Translate(Vector3.up * 0.01f);
				Colors[i] = Player.FactionID == faction.ID ?
					Selection.Entity == entity ? Settings.MainSelectionColor : Settings.AllyColor :
					Settings.EnemyColor;
				i++;
			}

			MaterialPropertyBlock.SetVectorArray(ShaderVariables.BaseColor, Colors);

			MeshDrawSettings settings = Settings.MeshDrawSettings;
			commandBuffer.DrawMeshInstanced(
				settings.Mesh,
				settings.SubmeshIndex,
				settings.Material,
				settings.ShaderPass,
				Matrices,
				Selection.Entities.Length,
				MaterialPropertyBlock);
		}
	}
}
