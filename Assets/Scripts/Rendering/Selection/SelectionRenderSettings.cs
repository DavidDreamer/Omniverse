using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Selection")]
	public class SelectionRenderSettings : CustomRendererConfig
	{
		[field: SerializeField]
		public MeshDrawSettings MeshDrawSettings { get; set; }

		[field: SerializeField]
		public Color MainSelectionColor { get; set; }

		[field: SerializeField]
		public Color AllyColor { get; set; }

		[field: SerializeField]
		public Color EnemyColor { get; set; }
	}
}
