using Dreambox.Rendering.Core;
using UnityEngine;

namespace Omniverse.Mapping
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Minimap")]
	public class MinimapRenderSettings : ScriptableObject
	{
		[field: SerializeField]
		public RenderTexture RenderTexture { get; private set; }

		[field: SerializeField]
		public Material Material { get; private set; }

		[field: SerializeField]
		public MeshDrawSettings MeshDrawSettings { get; private set; }
	}
}
