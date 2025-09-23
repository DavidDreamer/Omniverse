using Unity.Entities;
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

		public void Tick(EntityManager entityManager, Ability ability, int index)
		{
			bool isActiveAbility = ability.Desc.Value.ActiveOperation is not null;
			Hotkey.gameObject.SetActive(isActiveAbility);
			AbilityWidget.Tick(entityManager, ability, index);
		}
	}
}
