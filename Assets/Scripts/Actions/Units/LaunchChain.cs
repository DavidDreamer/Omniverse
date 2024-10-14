using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchChain : ScriptableObject, IAction<Unit, Unit>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public void Perform(Unit actor, Unit target)
		{
			actor.SpawnChain(Chain, target);
		}
	}
}