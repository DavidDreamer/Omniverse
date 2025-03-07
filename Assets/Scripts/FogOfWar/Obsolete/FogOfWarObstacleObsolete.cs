using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class FogOfWarObstacleObsolete : MonoBehaviour
	{
		[field: SerializeField]
		public Vector3 Size { get; private set; }

		[Inject]
		private FogOfWarObsolete FogOfWar { get; set; }

		private void OnEnable()
		{
			FogOfWar.AddObstacle(this);
		}
	}
}
