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
		private CastingWidget Casting { get; set; }

		[field: SerializeField]
		private CooldownWidget Cooldown { get; set; }

		[field: SerializeField]
		private AbilityTooltipWidget Tooltip { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		[field: SerializeField]
		private Image Activator { get; set; }

		[field: SerializeField]
		private Material DefaultMaterial { get; set; }

		[field: SerializeField]
		private Material OnCooldownMaterial { get; set; }

		[Inject]
		private AbilityController AbilityController { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		private Ability Ability { get; set; }

		public void Bind(Ability ability)
		{
			Ability = ability;

			Icon.sprite = ability.Desc.Meta.Icon;

			Casting.Bind(Ability.Casting);

			if (Ability.Cooldown is not null)
			{
				Cooldown.Bind(Ability.Cooldown);
			}

			Tooltip.Bind(Ability);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					AbilityController.Process(Selector.SelectedUnit, Ability);
					break;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Highlight.enabled = true;

			Tooltip.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Highlight.enabled = false;

			Tooltip.gameObject.SetActive(false);
		}

		public void Tick()
		{
			Activator.enabled = AbilityController.ActiveAbility == Ability;

			Casting.Tick();

			if (Ability.Cooldown is not null)
			{
				Icon.material = Ability.Cooldown.IsActive ? OnCooldownMaterial : DefaultMaterial;
				Cooldown.Tick();
			}
		}
	}
}
