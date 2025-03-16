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

		public void Tick(Ability ability)
		{
			Image.enabled = ability.IsOnCooldown;
			Image.fillAmount = ability.ActiveCooldownRatio;
			Label.enabled = ability.IsOnCooldown;
			Label.text = Mathf.CeilToInt(ability.ActiveCooldown).ToString();
		}
	}
}
