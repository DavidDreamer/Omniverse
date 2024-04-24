using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class Presentation
	{
		[field: SerializeField]
		public UnitRenderer Prefab { get; private set; }
		
		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}
