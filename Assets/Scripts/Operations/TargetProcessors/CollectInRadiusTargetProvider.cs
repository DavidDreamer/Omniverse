using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class CollectInRadiusTargetProvider : ITargetConverter<None, Entity>, ITargetConverter<Entity, Entity>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Entity> Convert(EntityManager entityManager, Entity actor, None input)
			=> PhysicsService.GetEntitiesInSphere(entityManager, actor, Radius, Filter);

		public IEnumerable<Entity> Convert(EntityManager entityManager, Entity actor, Entity input)
			=> PhysicsService.GetEntitiesInSphere(entityManager, input, Radius, Filter);
	}
}
