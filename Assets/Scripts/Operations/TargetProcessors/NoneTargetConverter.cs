using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class NoneTargetConverter : ITargetConverter<Entity, Entity>, ITargetConverter<ResourceSource, ResourceSource>, ITargetConverter<Vector3, Vector3>
	{
		public IEnumerable<Entity> Convert(EntityManager entityManager, Entity entity, Entity input)
		{
			yield return input;
		}

		public IEnumerable<Vector3> Convert(EntityManager entityManager, Entity entity, Vector3 input)
		{
			yield return input;
		}

		public IEnumerable<ResourceSource> Convert(EntityManager entityManager, Entity entity, ResourceSource input)
		{
			yield return input;
		}
	}
}
