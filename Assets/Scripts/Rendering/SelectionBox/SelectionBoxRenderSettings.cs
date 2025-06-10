using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/SelectionBox")]
	public class SelectionBoxRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public Material Material { get; set; }
	}
}
