using Omniverse.Abilities;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class AbilityWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private CooldownWidget Cooldown { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		[field: SerializeField]
		private Image Activator { get; set; }

		[field: SerializeField]
		private Material DefaultMaterial { get; set; }

		[field: SerializeField]
		private Material OnCooldownMaterial { get; set; }

		[Inject]
		private AbilityHandlerResolver AbilityHandlerResolver { get; set; }

		[Inject]
		private UnitSelector UnitSelector { get; set; }

		private Ability Ability { get; set; }

		public void Bind(Ability ability)
		{
			Ability = ability;

			Icon.sprite = ability.Desc.Presentation.Icon;

			if (Ability.Cooldown is not null)
			{
				Cooldown.Bind(Ability.Cooldown);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					AbilityHandlerResolver.TryCastAbility(UnitSelector.SelectedUnit, Ability);
					break;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Highlight.enabled = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Highlight.enabled = false;
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
	}
}
