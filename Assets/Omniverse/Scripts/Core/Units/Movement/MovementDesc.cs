using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Decs/Movement")]
	public class MovementDesc: ScriptableObject
	{
		[field: SerializeField]
		public float Speed { get; private set; }
		
		[field: SerializeField]
		public float RotationSpeed { get; private set; }
	}
}
