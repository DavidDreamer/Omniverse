using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class AbilitySlotWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Background { get; set; }

		[field: SerializeField]
		private AbilityWidget AbilityWidget { get; set; }

		[field: SerializeField]
		private HotkeyWidget Hotkey { get; set; }

		public void Initialize(InputAction inputAction)
		{
			Hotkey.Set(inputAction);
		}

		public void Bind(Ability ability)
		{
			bool hasAbility = ability is not null;
			bool isActiveAbility = hasAbility && ability.Desc.ActiveOperation is not null;

			AbilityWidget.gameObject.SetActive(hasAbility);
			Background.gameObject.SetActive(!hasAbility);
			Hotkey.gameObject.SetActive(isActiveAbility);

			if (hasAbility)
			{
				AbilityWidget.Bind(ability);
			}
		}

		public void Tick()
		{
			AbilityWidget.Tick();
		}
	}
}
