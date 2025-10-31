using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class LaunchChainAction : IAction<Entity>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public void Perform(EntityManager entityManager, Entity actor, Entity target)
		{
			//TODO ECS
			//actor.SpawnChain(Chain, target);
		}
	}
}
