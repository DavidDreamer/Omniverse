using Omniverse.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class PropertyBarWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Slider Slider { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Label { get; set; }
		
		private Property Property { get; set; }

		public void Bind(Property property)
		{
			Property = property;

			Slider.maxValue = property.Capacity.Value;
			Slider.value = property.Amount.Value;
		}

		public void Unbind()
		{
			Property = null;
		}

		protected virtual void LateUpdate()
		{
			if (Property is null)
			{
				return;
			}

			Slider.value = Property.Amount.Value;
			
			if (Label != null)
			{
				Label.text = $"{(int)Slider.value} / {Slider.maxValue}";
			}
		}
	}
}
