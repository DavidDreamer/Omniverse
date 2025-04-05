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

		public void Tick(EntityManager entityManager, Entity ability)
		{
			bool hasAbility = ability != Entity.Null;
			bool isActiveAbility = entityManager.HasComponent<AbilityActiveOperation>(ability);

			AbilityWidget.gameObject.SetActive(hasAbility);
			Background.gameObject.SetActive(!hasAbility);
			Hotkey.gameObject.SetActive(isActiveAbility);

			if (hasAbility)
			{
				AbilityWidget.Tick(ability);
			}
		}
	}
}
