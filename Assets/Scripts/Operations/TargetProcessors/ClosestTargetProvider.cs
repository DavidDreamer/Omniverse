using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ClosestTargetProvider : ITargetConverter<None, DynamicEntity>, ITargetConverter<DynamicEntity, DynamicEntity>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<DynamicEntity> Convert(EntityManager entityManager, DynamicEntity actor, None input)
		{
			yield return PhysicsService.GetClosestEntity(entityManager, actor, Radius, Filter);
		}

		public IEnumerable<DynamicEntity> Convert(EntityManager entityManager, DynamicEntity actor, DynamicEntity input)
		{
			yield return PhysicsService.GetClosestEntity(entityManager, input, Radius, Filter);
		}
	}
}
