using Dreambox.Rendering.Core;
using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Selection")]
	public class SelectionConfig: ScriptableObject
	{
		[field: SerializeField]
		public DrawMeshParams DrawMeshParams { get; set; }

		[field: SerializeField]
		public Vector3 Position{ get; set; }

		[field: SerializeField]
		public Vector3 Rotation { get; set; }

		[field: SerializeField]
		public Color AllyColor { get; set; }

		[field: SerializeField]
		public Color EnemyColor { get; set; }
	}
}
