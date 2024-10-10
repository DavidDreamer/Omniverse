using System;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class Meta
	{
		[field: SerializeField]
		public string Name{ get; private set; }

		[field: SerializeField]
		public string Description { get; private set; }

		[field: SerializeField]
		public Sprite Icon { get; private set; }
	}
}
