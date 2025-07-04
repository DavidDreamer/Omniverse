using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			public NavigationRenderSettings Settings;
			public Queue<NavigationPoint> Points;
			public NavigationPointDrawer Drawer;
		}

		private NavigationRenderSettings Settings { get; set; }

		private Queue<NavigationPoint> Points { get; }

		private NavigationPointDrawer Drawer { get; }

		public NavigationRenderPass(NavigationRenderSettings settings, Queue<NavigationPoint> poitns)
		{
			Settings = settings;
			Points = poitns;

			ConfigureInput(ScriptableRenderPassInput.Depth);

			Drawer = new NavigationPointDrawer(Settings.MeshDrawSettings, Settings.RenderingLayerMask, 4);
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("Navigation", out var data))
			{
				data.Settings = Settings;
				data.Points = Points;
				data.Drawer = Drawer;

				var universalResourceData = frameData.Get<UniversalResourceData>();
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0, AccessFlags.Write);
				builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.Read);

				builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
				{
					RasterCommandBuffer commandBuffer = context.cmd;

					double time = Time.time;
					foreach (NavigationPoint point in data.Points)
					{
						var matrix = point.Matrix;
						var lifetime = (float)math.clamp((time - point.Time) / (double)data.Settings.Lifetime, 0, 1);
						data.Drawer.Draw(commandBuffer, matrix, lifetime);
					}

					data.Drawer.Flush(commandBuffer);
				});
			}
		}
	}
}
