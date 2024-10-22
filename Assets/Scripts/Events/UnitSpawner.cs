using UnityEngine;
using VContainer;

namespace Omniverse.Entities.Units
{
	public class UnitSpawner : MonoBehaviour
	{
		[Inject]
		private UnitManager UnitManager { get; set; }

		public UnitDesc UnitDesc;

		public int FactionID;

		public void Start()
		{
			UnitManager.Spawn(UnitDesc, FactionID, transform.position);
		}
	}
}
