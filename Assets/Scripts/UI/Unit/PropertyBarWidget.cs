using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class PropertyBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Slider Slider { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		public void Tick(Health health)
		{
			Slider.maxValue = health.Maximum;
			Slider.value = health.Current;

			if (Label != null)
			{
				Label.text = $"{(int)Slider.value} / {Slider.maxValue}";
			}
		}

		public void Tick(Mana mana)
		{
			Slider.maxValue = mana.Maximum;
			Slider.value = mana.Current;

			if (Label != null)
			{
				Label.text = $"{(int)Slider.value} / {Slider.maxValue}";
			}
		}
	}
}
