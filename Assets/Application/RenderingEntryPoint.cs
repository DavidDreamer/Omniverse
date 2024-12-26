using Dreambox.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class RenderingEntryPoint : IInitializable
	{
		[Inject]
		private OutlineRenderer OutlineRenderer { get; set; }

		[Inject]
		private HealthBarsRenderer HealthBarsRenderer { get; set; }

		[Inject]
		private SelectionRenderer SelectionRenderer { get; set; }

		[Inject]
		private SelectionBoxRenderer SelectionBoxRenderer { get; set; }

		[Inject]
		private NavigationRenderer NavigationRenderer { get; set; }

		[Inject]
		private AbilityRenderer AbilityRenderer { get; set; }

		public void Initialize()
		{
		}
	}
}
