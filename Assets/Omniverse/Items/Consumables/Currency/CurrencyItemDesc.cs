using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class CurrencyItemDesc: ConsumableItemDesc
	{
		[field: SerializeField]
		[field: Resource]
		public int CurrencyID { get; private set; }
		
		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
