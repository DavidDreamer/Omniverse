using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class TeleportAction : IAction<Vector3>
	{
		public void Perform(EntityManager entityManager, DynamicEntity actor, Vector3 target)
		{
			actor.LocalTransform.ValueRW.Position = target;
		}
	}
}
