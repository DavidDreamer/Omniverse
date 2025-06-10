using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/FogOfWar")]
	public class FogOfWarRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public Material Material { get; private set; }

		[field: SerializeField]
		public Material BlurMaterial { get; private set; }
	}
}
