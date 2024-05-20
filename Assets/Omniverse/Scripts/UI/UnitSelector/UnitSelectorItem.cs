using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class UnitSelectorItem: MonoBehaviour
	{
		[field: SerializeField]
		public Image Icon { get; private set; }
		
		[field: SerializeField]
		public Image Selection { get; private set; }
	}
}
