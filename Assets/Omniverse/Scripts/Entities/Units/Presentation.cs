using System;
using UnityEngine;

namespace Omniverse.Entities.Units
{
	[Serializable]
	public class Presentation
	{
		[field: SerializeField]
		public Unit Prefab { get; private set; }
		
		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}
