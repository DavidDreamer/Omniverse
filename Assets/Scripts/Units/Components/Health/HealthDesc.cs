using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class HealthDesc
	{
		[field: SerializeField]
		public float Amount { get; private set; }
	}
}
