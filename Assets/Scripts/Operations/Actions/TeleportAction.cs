using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class TeleportAction : IAction<Vector3>
	{
		public void Perform(EntityManager entityManager, Entity actor, Vector3 target)
		{
			var localTransform = entityManager.GetComponentData<LocalTransform>(actor);
			localTransform.Position = target;
			entityManager.SetComponentData(actor, localTransform);
		}
	}
}
