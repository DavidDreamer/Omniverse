using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Omniverse
{
	public struct MetaData : IComponentData
	{
		public FixedString64Bytes Name;

		public WeakObjectReference<Sprite> Icon;

		public Sprite GetIcon()
		{
			Icon.LoadAsync();
			Icon.WaitForCompletion();
			return Icon.Result;
		}
	}
}
