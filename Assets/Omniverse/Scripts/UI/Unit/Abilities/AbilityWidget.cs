using Omniverse.Abilities;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class AbilityWidget: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[field: SerializeField]
		private Image Background { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Activator { get; set; }

		[field: SerializeField]
		private CooldownWidget Cooldown { get; set; }

		[field: SerializeField]
		private HotkeyWidget Hotkey { get; set; }
		
		[field: SerializeField]
		private Material DefaultMaterial { get; set; }
		
		[field: SerializeField]
		private Material OnCooldownMaterial { get; set; }
		
		[Inject]
		private AbilityHandlerResolver AbilityHandlerResolver { get; set; }
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }
		
		private Ability Ability { get; set; }

		public void Bind(Ability ability, InputAction inputAction)
		{
			Ability = ability;

			Background.gameObject.SetActive(false);
			Icon.gameObject.SetActive(true);
			
			Icon.sprite = ability.Desc.Presentation.Icon;
			Hotkey.Set(inputAction);

			if (Ability.Cooldown is not null)
			{
				Cooldown.Bind(Ability.Cooldown);
			}
		}

		public void Unbind()
		{
			Background.gameObject.SetActive(true);
			Ability = default;
		}

		public void Tick()
		{
			Activator.enabled = Ability.AwaitsTarget;
			
			if (Ability.Cooldown is not null)
			{
				Icon.material = Ability.Cooldown.IsActive ? OnCooldownMaterial : DefaultMaterial;
				Cooldown.Tick();
			}
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			AbilityHandlerResolver.TryCastAbility(UnitSelector.SelectedUnit.Unit, Ability);
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			Highlight.enabled = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Highlight.enabled = false;
		}
	}
}
