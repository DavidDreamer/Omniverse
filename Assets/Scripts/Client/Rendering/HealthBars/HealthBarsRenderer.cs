using Dreambox.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class HealthBarsRenderer : CustomRenderer<HealthBarsRendererConfig, HealthBarsRendererPass>
	{
		protected override HealthBarsRendererPass Setup() => new(this);

		protected override bool IsInactive() => false;
	}
}
