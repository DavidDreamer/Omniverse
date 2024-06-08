using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class PropertyWidget: MonoBehaviour
	{
		[field: SerializeField]
		public TextMeshProUGUI Value { get; private set; }
	}
}
