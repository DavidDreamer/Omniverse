using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ExtractResourceAction : IAction<ResourceSource>
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public void Perform(EntityManager entityManager, Entity actor, ResourceSource target)
		{
			//TODO ECS
			//actor.Extract(target, Amount);
		}
	}
}
