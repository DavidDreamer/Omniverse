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

			ConfigureInput(ScriptableRenderPassInput.Depth);

			SelectionDrawer = new(settings.MeshDrawSettings, settings.RenderingLayerMask, 64);
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
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0, AccessFlags.Write);
				builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.Read);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					foreach (Entity entity in data.Selection.Entities)
					{
						var localToWorld = data.EntityManager.GetComponentData<LocalToWorld>(entity);
						var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Scale(Vector3.one * data.Settings.SizeMultiplier);

						Color color;
						if (data.EntityManager.HasComponent<Faction>(entity))
						{
							var faction = data.EntityManager.GetComponentData<Faction>(entity);

							if (data.Player.FactionID == faction.ID)
							{
								if (data.Selection.Entity == entity)
								{
									color = data.Settings.MainSelectionColor;
								}
								else
								{
									color = data.Settings.AllyColor;
								}
							}
							else
							{
								color = data.Settings.EnemyColor;
							}

						}
						else
						{
							color = data.Settings.NeutralColor;
						}


						float radius = data.EntityManager.HasComponent<Building>(entity) ? data.Settings.BuildingRadius : data.Settings.UnitRadius;

						data.SelectionDrawer.Draw(commandBuffer, matrix, color, radius);
					}

					data.SelectionDrawer.Flush(commandBuffer);
				});
			}
		}
	}
}
