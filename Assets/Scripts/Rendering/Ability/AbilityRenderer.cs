using Dreambox.Rendering.Universal;
using Omniverse.Input;
using Unity.Entities;

namespace Omniverse.Rendering
{
	public class AbilityRenderer : CustomRenderer<AbilityRendererConfig, AbilityRendererPass>
	{
		protected override AbilityRendererPass Setup() => new(this);

		protected override bool IsInactive() => !ECSUtils.GetSingleton<AbilityInput>().InProcess;
	}
}
