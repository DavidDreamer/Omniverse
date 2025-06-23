using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class EffectWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Background { get; set; }

		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Timer { get; set; }

		[field: SerializeField]
		private Color ColorPositive { get; set; }

		[field: SerializeField]
		private Color ColorNegative { get; set; }

		public void Tick(Effect effect)
		{
			Icon.sprite = effect.Desc.Value.Icon;
			Background.color = effect.Desc.Value.IsPositive ? ColorPositive : ColorNegative;
			Timer.fillAmount = effect.Time / effect.Desc.Value.Duration;
		}
	}
}
