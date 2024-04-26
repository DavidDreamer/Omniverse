using UnityEngine;
using VContainer;

namespace Omniverse.Camera
{
	public class UnitSpawner: MonoBehaviour
	{
		[Inject]
		private UnitManager UnitManager { get; set; }

		public UnitDesc UnitDesc;

		public int FactionID;
		
		public void Start()
		{
			Unit unit = UnitManager.Spawn(UnitDesc, FactionID);
			unit.Presenter.NavMeshAgent.Warp(transform.position);
		}
	}
}
