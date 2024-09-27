using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;

namespace Omniverse.Rendering
{
	public class SelectionBoxRenderer : CustomRenderer<SelectionBoxRendererConfig, SelectionBoxRendererPass>
	{
		[Inject]
		public UnitSelector UnitSelector { get; private set; }

		protected override SelectionBoxRendererPass CreatePass() => new(this);

		protected override bool IsInactive() => !UnitSelector.InProcess;
	}
}
