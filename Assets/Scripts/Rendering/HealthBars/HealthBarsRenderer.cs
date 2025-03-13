using Dreambox.Rendering.Universal;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class HealthBarsRenderer : CustomRenderer<HealthBarsRendererConfig, HealthBarsRendererPass>, IInitializable
	{
		public void Initialize()
		{
		}

		protected override HealthBarsRendererPass Setup(HealthBarsRendererConfig config) => new(this);

		protected override bool IsInactive() => false;
	}
}
