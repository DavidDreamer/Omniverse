using UnityEngine;

namespace Omniverse
{
	public class TerrainPathfindingData : MonoBehaviour
	{
		[field: SerializeField]
		public float[] PenaltiesByLayer { get; private set; }
	}
}
