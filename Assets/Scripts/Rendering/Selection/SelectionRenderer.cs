using Dreambox.Rendering.Universal;
using Omniverse.Input;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class SelectionRenderer : CustomRenderer<SelectionRendererConfig, SelectionRendererPass>, IInitializable
	{
		[Inject]
		public Selector Selector { get; private set; }

		public Player Player { get; private set; }

		public void Initialize()
		{
			Player = ECSUtils.GetSingleton<Player>();
		}

		protected override SelectionRendererPass Setup(SelectionRendererConfig config) => new(this);

		protected override bool IsInactive() => false;
	}
}
