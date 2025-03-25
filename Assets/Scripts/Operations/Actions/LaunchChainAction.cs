using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class LaunchChainAction : IAction<DynamicEntity>
	{
		[field: SerializeField]
		public ChainDesc Chain { get; private set; }

		public void Perform(EntityManager entityManager, DynamicEntity actor, DynamicEntity target)
		{
			//TODO ECS
			//actor.SpawnChain(Chain, target);
		}
	}
}
