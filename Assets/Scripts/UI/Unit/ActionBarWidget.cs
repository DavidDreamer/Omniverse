using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ActionBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Slider Slider { get; set; }

		private Unit Unit { get; set; }

		public void Bind(Unit unit)
		{
			Unit = unit;
		}

		public void Tick()
		{
			var castAbilityCommand = Unit.CommandModule.Command as CastAbilityCommand;

			gameObject.SetActive(castAbilityCommand != null);

			if (castAbilityCommand == null)
			{
				return;
			}

			Icon.sprite = castAbilityCommand.Ability.Desc.Meta.Icon;
			Slider.minValue = 0f;
			Slider.maxValue = castAbilityCommand.Ability.Desc.Casting.Time;
			Slider.value = castAbilityCommand.Ability.Casting.Time;
		}
	}
}
