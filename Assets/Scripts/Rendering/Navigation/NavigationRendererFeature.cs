using System.Collections.Generic;
using Dreambox.Rendering.URP;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class NavigationRendererFeature : RendererFeature<NavigationRenderConfig, NavigationRenderPass>
	{
		public Queue<NavigationPoint> Points { get; } = new();

		public override NavigationRenderPass CreatePass() => new(this);

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (Points.Count == 0)
			{
				return;
			}

			base.AddRenderPasses(renderer, ref renderingData);
		}
	}
}
