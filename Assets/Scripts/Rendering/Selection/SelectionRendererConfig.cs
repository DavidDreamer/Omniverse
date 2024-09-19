using Dreambox.Rendering.Core;
using Dreambox.Rendering.Universal;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Selection")]
	public class SelectionRendererConfig : CustomRendererConfig
	{
		[field: SerializeField]
		public DrawMeshParams DrawMeshParams { get; set; }

		[field: SerializeField]
		public Color MainSelectionColor { get; set; }

		[field: SerializeField]
		public Color AllyColor { get; set; }

		[field: SerializeField]
		public Color EnemyColor { get; set; }
	}
}
