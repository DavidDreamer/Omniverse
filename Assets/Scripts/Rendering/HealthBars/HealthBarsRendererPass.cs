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

	public class HealthBarsRendererPass : ScriptableRenderPass, IDisposable
	{
		private class PassData
		{
		}

		private HealthBarsRenderer Renderer { get; }

		private HealthBarsRendererConfig Config { get; }

		private HealthBarDrawer HealthBarDrawer { get; }

		public HealthBarsRendererPass(HealthBarsRenderer renderer)
		{
			Renderer = renderer;
			Config = renderer.Config;

			HealthBarDrawer = new HealthBarDrawer(Config.DrawMeshParams, 64);
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

			var player = ECSUtils.GetSingleton<Player>();

			var entityManager = ECSUtils.ClientWorld.EntityManager;
			var query = entityManager.CreateEntityQuery(typeof(Health));
			var entities = query.ToEntityArray(Allocator.Temp);

			int drawnCount = 0;

			while (drawnCount < entities.Length)
			{
				int currentBatchSize = Math.Min(entities.Length - drawnCount, HealthBarDrawer.BatchSize);

				for (int i = drawnCount; i < currentBatchSize; i++)
				{
					Entity entity = entities[i];

					var health = entityManager.GetComponentData<Health>(entity);
					var faction = entityManager.GetComponentData<Faction>(entity);
					var localToWorld = entityManager.GetComponentData<LocalToWorld>(entity);

					var matrix = (Matrix4x4)localToWorld.Value * Matrix4x4.Translate(Config.Offset);

					HealthBarColors colors = player.FactionID == faction.ID ? Config.AllyColors : Config.EnemyColors;
					Color baseColor = colors.BaseColor;
					Color secondColor = colors.SecondColor;
					float amount = health.Current / health.Maximum;

					HealthBarDrawer.AddInstance(matrix, baseColor, secondColor, amount);
				}

				HealthBarDrawer.DrawBatch(commandBuffer);

				drawnCount += currentBatchSize;
			}
		}
	}
}
