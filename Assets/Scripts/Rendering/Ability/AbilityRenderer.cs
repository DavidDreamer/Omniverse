using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;
using VContainer.Unity;

namespace Omniverse.Rendering
{
	public class AbilityRenderer : CustomRenderer<AbilityRendererConfig, AbilityRendererPass>, IInitializable
	{
		public void Initialize()
		{
		}

		protected override AbilityRendererPass Setup(AbilityRendererConfig config) => new(this);

		protected override bool IsInactive() => ECSUtils.GetSingleton<AbilityInput>().Ability == Entity.Null;
	}
}
