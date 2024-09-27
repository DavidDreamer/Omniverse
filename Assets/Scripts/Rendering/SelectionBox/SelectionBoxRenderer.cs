using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;

namespace Omniverse.Rendering
{
	public class SelectionBoxRenderer : CustomRenderer<SelectionBoxRendererConfig, SelectionBoxRendererPass>
	{
		[Inject]
		public Selector Selector { get; private set; }

		protected override SelectionBoxRendererPass CreatePass() => new(this);

		protected override bool IsInactive() => !Selector.InProcess;
	}
}
