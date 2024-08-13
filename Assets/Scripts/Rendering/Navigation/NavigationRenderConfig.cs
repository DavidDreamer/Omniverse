using Dreambox.Rendering.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Navigation")]
	public class NavigationRenderConfig : ScriptableObject
	{
		[field: SerializeField]
		public RenderPassEvent RenderPassEvent { get; set; }

		[field: SerializeField]
		public DrawMeshParams DrawMeshParams { get; set; }

		[field: SerializeField]
		public int Capacity { get; private set; }

		[field: SerializeField]
		public float Lifetime { get; private set; }
	}
}
