using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Selection")]
	public class SelectionConfig: ScriptableObject
	{
		[field: SerializeField]
		public Mesh Mesh{ get; set; }

		[field: SerializeField]
		public Material Material{ get; set; }

		[field: SerializeField]
		public int ShaderPass{ get; set; }

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
