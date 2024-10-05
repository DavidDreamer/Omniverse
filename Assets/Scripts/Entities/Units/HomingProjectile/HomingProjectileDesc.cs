using UnityEngine;

namespace Omniverse.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Homing Projectile")]
	public class HomingProjectileDesc : EntityDesc
	{
		[field: SerializeField]
		public float Speed { get; private set; }

		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		[field: OperationPicker]
		public Operation HitOperation { get; private set; }
	}
}
