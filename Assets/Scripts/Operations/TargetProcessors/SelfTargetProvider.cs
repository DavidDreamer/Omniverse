using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetConverter<None, Entity>, ITargetConverter<None, Vector3>
	{
		IEnumerable<Entity> ITargetConverter<None, Entity>.Convert(EntityManager entityManager, Entity actor, None input)
		{
			yield return actor;
		}

		IEnumerable<Vector3> ITargetConverter<None, Vector3>.Convert(EntityManager entityManager, Entity actor, None input)
		{
			var localTranform = entityManager.GetComponentData<LocalTransform>(actor);
			yield return localTranform.Position;
		}
	}
}
