using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Builder")]
	public class BuilderRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public Material GridMaterial { get; private set; }

		[field: SerializeField]
		public Material Material { get; private set; }
	}
}
