using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Camera
{
	public class UnitSpawner: MonoBehaviour
	{
		[Inject]
		private UnitManager UnitManager { get; set; }

		public UnitDesc UnitDesc;

		public void Start()
		{
			Spawn(0);
			Spawn(0);
			Spawn(1);
			Spawn(1);

			void Spawn(int factionID)
			{
				Unit unit = UnitManager.Spawn(UnitDesc, factionID);
				unit.Presenter.NavMeshAgent.Warp(transform.position);
			}
		}
	}
}
