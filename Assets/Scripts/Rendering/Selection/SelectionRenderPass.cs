using Dreambox.Core;
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
		private class PassData
		{
			public EntityManager EntityManager;
			public Player Player;
			public Selection Selection;
			public SelectionRenderSettings Settings;
			public SelectionDrawer SelectionDrawer;
		}

		public Player Player { get; set; }

		public Selection Selection { get; set; }

		private SelectionRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private SelectionDrawer SelectionDrawer { get; }

		public SelectionRenderPass(SelectionRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;

			SelectionDrawer = new(settings.MeshDrawSettings, 64);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Selection", out var data))
			{
				data.EntityManager = EntityManager;
				data.Player = Player;
				data.Selection = Selection;
				data.Settings = Settings;
				data.SelectionDrawer = SelectionDrawer;

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);
				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					foreach (Entity entity in data.Selection.Entities)
					{
						var localToWorld = data.EntityManager.GetComponentData<LocalToWorld>(entity);
						var faction = data.EntityManager.GetComponentData<Faction>(entity);
						var matrix = (Matrix4x4)localToWorld.Value * MatrixUtils.WorldUpRotation * Matrix4x4.Translate(Vector3.up * 0.01f);
						matrix *= Matrix4x4.Scale(Vector3.one * data.Settings.SizeMultiplier);
						Color color = data.Player.FactionID == faction.ID ?
							data.Selection.Entity == entity ? data.Settings.MainSelectionColor : data.Settings.AllyColor :
							data.Settings.EnemyColor;
						float radius = data.EntityManager.HasComponent<Building>(entity) ? data.Settings.BuildingRadius : data.Settings.UnitRadius;

						data.SelectionDrawer.Draw(commandBuffer, matrix, color, radius);
					}

					data.SelectionDrawer.Flush(commandBuffer);
				});
			}
		}
	}
}
