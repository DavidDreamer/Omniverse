using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetConverter<None, DynamicEntity>, ITargetConverter<None, Vector3>
	{
		IEnumerable<DynamicEntity> ITargetConverter<None, DynamicEntity>.Convert(EntityManager entityManager, DynamicEntity actor, None input)
		{
			yield return actor;
		}

		IEnumerable<Vector3> ITargetConverter<None, Vector3>.Convert(EntityManager entityManager, DynamicEntity actor, None input)
		{
			yield return actor.LocalTransform.ValueRO.Position;
		}
	}
}
