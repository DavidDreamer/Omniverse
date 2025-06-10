using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Navigation")]
	public class NavigationRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings DrawMeshParams { get; set; }

		[field: SerializeField]
		public int Capacity { get; private set; }

		[field: SerializeField]
		public float Lifetime { get; private set; }
	}
}
