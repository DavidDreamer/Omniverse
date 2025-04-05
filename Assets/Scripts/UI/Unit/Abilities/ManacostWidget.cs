using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class ManacostWidget : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		private Manacost Manacost { get; set; }

		public void Tick(Manacost manacost)
		{
			Manacost = manacost;

			Label.text = manacost.Value.ToString();
		}
	}
}
