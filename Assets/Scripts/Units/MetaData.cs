using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public struct MetaData : IComponentData
	{
		public FixedString64Bytes Name;

		public UnityObjectRef<Sprite> Icon;
	}
}
