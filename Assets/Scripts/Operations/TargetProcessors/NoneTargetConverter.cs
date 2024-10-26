using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class NoneTargetConverter : ITargetConverter<Unit, Unit>, ITargetConverter<ResourceSource, ResourceSource>, ITargetConverter<Vector3, Vector3>
	{
		public IEnumerable<Unit> Convert(Entity entity, Unit input)
		{
			yield return input;
		}

		public IEnumerable<Vector3> Convert(Entity entity, Vector3 input)
		{
			yield return input;
		}

		public IEnumerable<ResourceSource> Convert(Entity entity, ResourceSource input)
		{
			yield return input;
		}
	}
}
