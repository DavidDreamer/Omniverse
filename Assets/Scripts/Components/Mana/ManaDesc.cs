using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ManaDesc
	{
		[field: SerializeField]
		public float Amount { get; private set; }
	}
}
