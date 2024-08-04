using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Rendering/Health Bar", fileName = nameof(HealthBarConfig))]
	public class HealthBarConfig: ScriptableObject
	{
		[field: SerializeField]
		public int MaxCount { get; set; }

		[field: SerializeField]
		public Mesh Mesh { get; set; }

		[field: SerializeField]
		public Material Material { get; set; }

		[field: SerializeField]
		public int ShaderPass { get; set; }

		[field: SerializeField]
		public Vector3 Offset { get; set; }

		[field: SerializeField]
		public HealthBarColors AllyColors { get; private set; }
		
		[field: SerializeField]
		public HealthBarColors EnemyColors { get; private set; }
	}
}
