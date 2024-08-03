using Dreambox.Math;
using UnityEngine;

namespace Omniverse.Entities.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Projectile")]
	public class ProjectileDesc : EntityDesc
	{
		[field: SerializeField]
		public float Range { get; private set; }

		[field: SerializeField]
		public float Speed { get; private set; }

		[field: SerializeField]
		public float Radius { get; private set; }
	}
}
