using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class LaunchChain : IAction<Entity>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public void Perform(Entity actor, Entity target)
		{
			actor.SpawnChain(Chain, target);
		}
	}
}
