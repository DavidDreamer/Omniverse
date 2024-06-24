using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class AbilitySlotWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Background { get; set; }
		
		[field: SerializeField]
		private AbilityWidget AbilityWidget { get; set; }

		[field: SerializeField]
		private HotkeyWidget Hotkey { get; set; }

		public void Bind(Ability ability, InputAction inputAction)
		{
			bool hasAbility = ability is not null;
			AbilityWidget.gameObject.SetActive(hasAbility);
			Background.gameObject.SetActive(!hasAbility);
			Hotkey.gameObject.SetActive(hasAbility);
			
			if (hasAbility)
			{
				AbilityWidget.Bind(ability);
				Hotkey.Set(inputAction);
			}
		}

		public void Tick()
		{
			AbilityWidget.Tick();
		}
	}
}
