using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Collector Settings")]
	public class CollectorSettings: ScriptableObject
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public LayerMask LayerMask { get; private set; }
		
		[field: SerializeField]
		public int Capacity { get; private set; }
	}
}
