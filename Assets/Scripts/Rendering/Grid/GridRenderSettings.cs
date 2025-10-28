using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Grid")]
	public class GridRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public Material Material { get; private set; }
	}
}
