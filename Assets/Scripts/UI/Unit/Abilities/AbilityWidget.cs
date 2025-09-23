using Omniverse.Abilities;
using Omniverse.Input;
using Unity.Entities;
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
		private ManacostWidget Manacost { get; set; }

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

		public void Tick(EntityManager entityManager, Ability ability, int index)
		{
			var selection = entityManager.CreateEntityQuery(typeof(Selection)).GetSingleton<Selection>();

			Activator.enabled = selection.AbilityInProcess && selection.AbilityIndex == index;

			Icon.sprite = ability.Desc.Value.Meta.Icon;
			Icon.material = ability.Cooldown.Active ? OnCooldownMaterial : DefaultMaterial;

			Casting.Tick(ability.Casting);

			Cooldown.Tick(entityManager, ability.Cooldown);
			Tooltip.Bind(ability.Desc.Value.Meta.Name);

			bool hasManacost = ability.Manacost.Value > 0;
			Manacost.gameObject.SetActive(hasManacost);
			if (hasManacost)
			{
				Manacost.Tick(ability.Manacost);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					//TODO ECS
					//var selection = ECSUtils.GetSingleton<Selection>();
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
