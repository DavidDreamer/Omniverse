using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/SelectionBox")]
	public class SelectionBoxRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public Material Material { get; set; }
	}
}
