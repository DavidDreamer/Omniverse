using Omniverse.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

		private Ability Ability { get; set; }

		public void Tick(Ability ability)
		{
			Ability = ability;

			var abilityInput = ECSUtils.GetSingletonManaged<AbilityInput>();

			Activator.enabled = abilityInput.Ability == ability;

			Icon.sprite = ability.MetaData.GetIcon();
			Icon.material = Ability.Cooldown.IsActive ? OnCooldownMaterial : DefaultMaterial;

			Casting.Tick(Ability.Casting);

			Cooldown.Tick(ability.Cooldown);
			Tooltip.Bind(ability);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					var selection = ECSUtils.GetSingleton<Selection>();
					//TODO ECS
					//AbilityController.Process(selection.Entity, Ability);
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
	}
}
