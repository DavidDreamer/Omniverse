using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Navigation")]
	public class NavigationRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings MeshDrawSettings { get; set; }

		[field: SerializeField]
		public int Capacity { get; private set; }

		[field: SerializeField]
		public float Lifetime { get; private set; }
	}
}
