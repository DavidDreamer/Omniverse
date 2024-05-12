using UnityEngine;
using VContainer;

namespace Omniverse.FogOfWar
{
	public class FogOfWarObstacle: MonoBehaviour
	{
		[field: SerializeField]
		public Vector3 Size { get; private set; }

		[Inject]
		private FogOfWarManager FogOfWar { get; set; }

		private void OnEnable()
		{
			FogOfWar.AddObstacle(this);
		}
	}
}
