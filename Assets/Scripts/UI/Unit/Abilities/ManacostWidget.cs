using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class ManacostWidget : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		public void Tick(Manacost manacost)
		{
			Label.text = manacost.Value.ToString();
		}
	}
}
