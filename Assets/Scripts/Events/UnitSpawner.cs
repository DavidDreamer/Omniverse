using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Entities.Units
{
	public class UnitSpawner : MonoBehaviour
	{
		[Inject]
		private Manager UnitManager { get; set; }

		public UnitDesc UnitDesc;

		public int FactionID;

		public void Start()
		{
			UnitManager.Spawn(UnitDesc, FactionID, transform.position);
		}
	}
}
