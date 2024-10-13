using Omniverse.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameDirector : IFixedTickable
	{
		[Inject]
		private FogOfWar FogOfWar { get; set; }

		[Inject]
		private TempNameManager TempNameManager { get; set; }

		[Inject]
		private UnitManager UnitManager { get; set; }

		public void FixedTick()
		{
			float deltaTime = Time.fixedDeltaTime;

			FogOfWar.Tick();
			TempNameManager.Tick(deltaTime);
			UnitManager.Tick(deltaTime);
			UnitManager.UpdateLivingState();
		}
	}
}
