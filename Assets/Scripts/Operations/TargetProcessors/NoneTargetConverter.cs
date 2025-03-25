using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class NoneTargetConverter : ITargetConverter<DynamicEntity, DynamicEntity>, ITargetConverter<ResourceSource, ResourceSource>, ITargetConverter<Vector3, Vector3>
	{
		public IEnumerable<DynamicEntity> Convert(EntityManager entityManager, DynamicEntity entity, DynamicEntity input)
		{
			yield return input;
		}

		public IEnumerable<Vector3> Convert(EntityManager entityManager, DynamicEntity entity, Vector3 input)
		{
			yield return input;
		}

		public IEnumerable<ResourceSource> Convert(EntityManager entityManager, DynamicEntity entity, ResourceSource input)
		{
			yield return input;
		}
	}
}
