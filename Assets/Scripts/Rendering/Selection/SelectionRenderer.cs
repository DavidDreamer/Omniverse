using Dreambox.Rendering.Universal;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class SelectionRenderer : CustomRenderer<SelectionRendererConfig, SelectionRendererPass>, IInitializable
	{
		public void Initialize()
		{
		}

		protected override SelectionRendererPass Setup(SelectionRendererConfig config) => new(this);

		protected override bool IsInactive() => false;
	}
}
