using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchChain : Action<Unit, Unit>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public override void Perform(Unit actor, Unit target)
		{
			actor.SpawnChain(Chain, target);
		}
	}
}