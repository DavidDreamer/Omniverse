using UnityEngine;
using VContainer;

namespace Omniverse.Units
{
	public class UnitSpawner: MonoBehaviour
	{
		[Inject]
		private Manager UnitManager { get; set; }

		public UnitDesc UnitDesc;

		public int FactionID;
		
		public void Start()
		{
			Unit unit = UnitManager.Spawn(UnitDesc, FactionID);
			unit.Presenter.NavMeshAgent.Warp(transform.position);
		}
	}
}
