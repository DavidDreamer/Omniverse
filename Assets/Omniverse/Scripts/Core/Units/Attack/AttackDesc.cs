using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Attack")]
	public class AttackDesc: ScriptableObject
	{
		[field: SerializeField]
		public float Range { get; private set; }
		
		[field: SerializeField]
		public float Speed { get; private set; }
		
		[field: SerializeField]
		public float Damage { get; private set; }
	}
}
