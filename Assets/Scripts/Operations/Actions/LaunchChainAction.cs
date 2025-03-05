using UnityEngine;

namespace Omniverse
{
	public class LaunchChainAction : IAction<OmniverseEntity>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public void Perform(OmniverseEntity actor, OmniverseEntity target)
		{
			actor.SpawnChain(Chain, target);
		}
	}
}
