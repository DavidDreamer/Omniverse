using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class CooldownWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Image { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		public void Tick(Cooldown cooldown)
		{
			Image.enabled = cooldown.IsActive();
			Image.fillAmount = cooldown.Ratio();
			Label.enabled = cooldown.IsActive();
			Label.text = Mathf.CeilToInt(cooldown.TimeLeft).ToString();
		}
	}
}
