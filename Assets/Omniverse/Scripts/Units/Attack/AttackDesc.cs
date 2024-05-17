using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class AttackDesc
	{
		[field: SerializeField]
		public float Range { get; private set; }

		[field: SerializeField]
		public float Speed { get; private set; }

		[field: SerializeField]
		public float Damage { get; private set; }
	}
}
