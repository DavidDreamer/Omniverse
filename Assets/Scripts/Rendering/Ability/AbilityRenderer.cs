using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;

namespace Omniverse.Rendering
{
	public class AbilityRenderer : CustomRenderer<AbilityRendererConfig, AbilityRendererPass>
	{
		[Inject]
		public AbilityController AbilityController { get; private set; }

		protected override AbilityRendererPass CreatePass() => new(this);

		protected override bool IsInactive() => AbilityController.ActiveAbility is null;
	}
}
