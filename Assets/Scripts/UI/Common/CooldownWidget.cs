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

		public void Tick(EntityManager entityManager, Entity ability)
		{
			var cooldown = entityManager.GetComponentData<Cooldown>(ability);
			var isEnabled = entityManager.IsComponentEnabled<Cooldown>(ability);

			Image.enabled = isEnabled;
			Image.fillAmount = cooldown.Ratio();
			Label.enabled = isEnabled;
			Label.text = Mathf.CeilToInt(cooldown.TimeLeft).ToString();
		}
	}
}
