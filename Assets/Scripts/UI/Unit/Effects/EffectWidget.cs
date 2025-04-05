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

		private Effect Effect { get; set; }

		public void Tick(Effect effect)
		{
			//TODO ECS
			//Icon.sprite = Effect.Desc.Icon;
			//Background.color = Effect.Desc.IsPositive ? ColorPositive : ColorNegative;
			//Timer.fillAmount = Effect.Time / Effect.Desc.Time;
		}
	}
}
