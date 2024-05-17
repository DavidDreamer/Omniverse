using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class StatWidget: MonoBehaviour
	{
		[field: SerializeField]
		public TextMeshProUGUI Value { get; private set; }
	}
}
