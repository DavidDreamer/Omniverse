using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class Presentation
	{
		[field: SerializeField]
		public UnitRendererBase Prefab { get; private set; }
		
		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}
