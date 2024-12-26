using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;

namespace Omniverse.Rendering
{
	public class AbilityRenderer : CustomRenderer<AbilityRendererConfig, AbilityRendererPass>
	{
		[Inject]
		public InputController InputController { get; private set; }

		[Inject]
		public AbilityController AbilityController { get; private set; }

		protected override AbilityRendererPass Setup(AbilityRendererConfig config) => new(this);

		protected override bool IsInactive() => AbilityController.ActiveAbility is null;
	}
}
