using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class HealthBarRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			public EntityManager EntityManager;
			public HealthBarDrawer HealthBarDrawer;
			public Player Player;
			public HealthBarRenderSettings Settings;
		}

		public Player Player { get; set; }

		private HealthBarRenderSettings Settings { get; }

		private EntityManager EntityManager { get; }

		private HealthBarDrawer HealthBarDrawer { get; }

		public HealthBarRenderPass(HealthBarRenderSettings settings, EntityManager entityManager)
		{
			Settings = settings;
			EntityManager = entityManager;

			HealthBarDrawer = new HealthBarDrawer(Settings.MeshDrawSettings, 64);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Health Bars", out var data))
			{
				data.EntityManager = EntityManager;
				data.HealthBarDrawer = HealthBarDrawer;
				data.Player = Player;
				data.Settings = Settings;

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					var query = data.EntityManager.CreateEntityQuery(typeof(Health));
					var entities = query.ToEntityArray(Allocator.Temp);

					int drawnCount = 0;

					while (drawnCount < entities.Length)
					{
						int currentBatchSize = Math.Min(entities.Length - drawnCount, data.HealthBarDrawer.BatchSize);

						for (int i = drawnCount; i < currentBatchSize; i++)
						{
							Entity entity = entities[i];

							var health = data.EntityManager.GetComponentData<Health>(entity);
							var faction = data.EntityManager.GetComponentData<Faction>(entity);
							var localToWorld = data.EntityManager.GetComponentData<LocalToWorld>(entity);

							var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Translate(data.Settings.Offset);

							HealthBarColors colors = data.Player.FactionID == faction.ID ? data.Settings.AllyColors : data.Settings.EnemyColors;
							Color baseColor = colors.BaseColor;
							Color secondColor = colors.SecondColor;
							float amount = health.Current / health.Maximum;

							data.HealthBarDrawer.AddInstance(matrix, baseColor, secondColor, amount);
						}

						data.HealthBarDrawer.DrawBatch(commandBuffer);

						drawnCount += currentBatchSize;
					}
				});
			}
		}
	}
}
