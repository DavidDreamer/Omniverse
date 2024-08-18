using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Navigation")]
	public class NavigationRenderConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public DrawMeshParams DrawMeshParams { get; set; }

		[field: SerializeField]
		public int Capacity { get; private set; }

		[field: SerializeField]
		public float Lifetime { get; private set; }
	}
}
