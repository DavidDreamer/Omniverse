using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/FogOfWar")]
	public class FogOfWarRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public Material Material { get; private set; }

		[field: SerializeField]
		public Material BlurMaterial { get; private set; }
	}
}
