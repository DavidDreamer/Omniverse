using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class FogOfWarObstacle : MonoBehaviour
	{
		[field: SerializeField]
		public Vector3 Size { get; private set; }

		[Inject]
		private FogOfWar FogOfWar { get; set; }

		private void OnEnable()
		{
			FogOfWar.AddObstacle(this);
		}
	}
}
