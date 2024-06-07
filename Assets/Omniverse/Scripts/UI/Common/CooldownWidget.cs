using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class CooldownWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Image { get; set; }
		
		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }

		private Abilities.Cooldown Cooldown { get; set; }
		
		public void Bind(Abilities.Cooldown cooldown)
		{
			Cooldown = cooldown;
		}

		public void Tick()
		{
			Image.fillAmount = Cooldown.TimeLeftRatio;
			Label.enabled = Cooldown.IsActive;
			Label.text = Mathf.CeilToInt(Cooldown.TimeLeft).ToString();
		}
	}
}
