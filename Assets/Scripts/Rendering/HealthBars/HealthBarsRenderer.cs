using Dreambox.Rendering.Universal;
using VContainer;

namespace Omniverse.Rendering
{
	public class HealthBarsRenderer : CustomRenderer<HealthBarsRendererConfig, HealthBarsRendererPass>
	{
		[Inject]
		public UnitManager UnitManager { get; private set; }

		[Inject]
		public Player Player { get; private set; }

		protected override HealthBarsRendererPass CreatePass() => new(this);

		protected override bool IsInactive() => false;
	}
}
