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
			Spawn();
			Spawn();
			Spawn();

			void Spawn()
			{
				Unit unit = UnitManager.Spawn(UnitDesc, 0);
				unit.Presenter.NavMeshAgent.Warp(transform.position);
			}
		}
	}
}
