using Omniverse.Entities.Units;
using TMPro;
using UnityEngine;
using VContainer;

namespace Omniverse.UI
{
	public class PropertyWidget: MonoBehaviour
	{
		[field: SerializeField]
		public TextMeshProUGUI Value { get; private set; }
		
		private Property Property { get; set; }
		
		[Inject]
		private UIStyle Style { get; set; }

		public void Bind(Property property)
		{
			Property = property;
		}

		public void LateUpdate()
		{
			if (Property is null)
			{
				return;
			}
			
			Value.text = Property.Amount.Value.ToString();
			Value.color = GetColor();
		}

		private Color GetColor()
		{
			float rawAmount = Property.RawAmount.Value;
			float amount = Property.Amount.Value;
			
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
