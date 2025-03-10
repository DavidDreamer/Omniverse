using Dreambox.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class HealthBarsRenderer : CustomRenderer<HealthBarsRendererConfig, HealthBarsRendererPass>, IInitializable
	{
		[Inject]
		public UnitManager UnitManager { get; private set; }

		public Player Player { get; private set; }

		public void Initialize()
		{
			Player = ECSUtils.GetSingleton<Player>();
		}

		protected override HealthBarsRendererPass Setup(HealthBarsRendererConfig config) => new(this);

		protected override bool IsInactive() => false;
	}
}
