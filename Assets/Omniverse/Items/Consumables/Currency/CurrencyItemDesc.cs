using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu]
	public class CurrencyItemDesc: ItemDesc
	{
		[field: SerializeField]
		[field: Currency]
		public int CurrencyID { get; private set; }
		
		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
