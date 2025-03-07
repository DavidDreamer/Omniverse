using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameDirector : IFixedTickable
	{
		[Inject]
		private TempNameManager TempNameManager { get; set; }

		[Inject]
		private UnitManager UnitManager { get; set; }

		public void FixedTick()
		{
			float deltaTime = Time.fixedDeltaTime;

			TempNameManager.Tick(deltaTime);
			UnitManager.Tick(deltaTime);
			UnitManager.UpdateLivingState();
		}
	}
}
