using System;
using UnityEngine;

namespace Omniverse
{
	public class ResourceSource: Entity
	{
		[field: SerializeField]
		public ResourceSourceDesc Desc { get; private set; }
		
		public int Amount { get; private set; }

		private void Awake()
		{
			Amount = Desc.Amount;
		}

		public void ChangeAmount(int delta)
		{
			Amount += delta;
		}
	}
}
