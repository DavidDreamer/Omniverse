using UnityEngine;

namespace Omniverse.Units.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Client/Health Bar", fileName = nameof(HealthBarConfig))]
	public class HealthBarConfig: ScriptableObject
	{
		[field: SerializeField]
		public HealthBarColors AllyColors { get; private set; }
		
		[field: SerializeField]
		public HealthBarColors EnemyColors { get; private set; }
	}
}
