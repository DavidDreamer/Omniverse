using Omniverse.Entities.Units.Client;
using UnityEngine;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Config/UnitSelector")]
	public class UnitSelectorConfig: ScriptableObject
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
