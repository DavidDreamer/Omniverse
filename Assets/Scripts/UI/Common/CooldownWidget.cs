using TMPro;
using Unity.Entities;
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

		public void Tick(EntityManager entityManager, Cooldown cooldown)
		{
			Image.enabled = cooldown.Active;
			Image.fillAmount = cooldown.Ratio();
			Label.enabled = cooldown.Active;
			Label.text = Mathf.CeilToInt(cooldown.TimeLeft).ToString();
		}
	}
}
