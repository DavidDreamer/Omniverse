using Dreambox.Rendering.Universal;

namespace Omniverse.Rendering
{
	public class SelectionRenderer : CustomRenderer<SelectionRendererConfig, SelectionRendererPass>
	{
		protected override SelectionRendererPass Setup() => new(this);

		protected override bool IsInactive() => false;
	}
}
