using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;

namespace Omniverse.Rendering
{
	public class SelectionRenderer : CustomRenderer<SelectionRendererConfig, SelectionRendererPass>
	{
		[Inject]
		public UnitSelector UnitSelector { get; private set; }

		[Inject]
		public Player Player { get; private set; }

		protected override SelectionRendererPass CreatePass() => new(this);

		protected override bool IsInactive() => false;
	}
}
