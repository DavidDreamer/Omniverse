using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class AbilityRenderer : CustomRenderer<AbilityRendererConfig, AbilityRendererPass>, IInitializable
	{
		[Inject]
		public InputController InputController { get; private set; }

		[Inject]
		public AbilityController AbilityController { get; private set; }

		public void Initialize()
		{
		}

		protected override AbilityRendererPass Setup(AbilityRendererConfig config) => new(this);

		protected override bool IsInactive() => AbilityController.ActiveAbility is null;
	}
}
