using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class PropertyWidget : MonoBehaviour
	{
		[field: SerializeField]
		public TextMeshProUGUI Value { get; private set; }

		[field: SerializeField]
		public UIStyle Style { get; private set; }

		private Property Property { get; set; }

		public void Bind(Property property)
		{
			Property = property;
		}

		public void LateUpdate()
		{
			Value.text = Property.Base.ToString();
			Value.color = GetColor();
		}

		private Color GetColor()
		{
			float rawAmount = Property.Base;
			float amount = Property.Total;

			if (amount > rawAmount)
			{
				return Style.Text.PositiveColor;
			}

			if (amount < rawAmount)
			{
				return Style.Text.NegativeColor;
			}

			return Style.Text.DefaultColor;
		}
	}
}
