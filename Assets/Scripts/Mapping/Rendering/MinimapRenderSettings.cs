using Dreambox.Rendering.Core;
using UnityEngine;

namespace Omniverse.Mapping
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Minimap")]
	public class MinimapRenderSettings : ScriptableObject
	{
		[field: SerializeField]
		public Material FrustrumMaterial { get; private set; }

		[field: SerializeField]
		public MeshDrawSettings DrawMeshParams { get; private set; }
	}
}
