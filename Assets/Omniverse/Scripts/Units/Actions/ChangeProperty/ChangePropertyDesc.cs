using System;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public class ChangePropertyDesc: IActionDesc
	{
		[field: SerializeField]
		public PropertyID PropertyID { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
