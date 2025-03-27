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
		private AbilityTooltipWidget Tooltip { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		[field: SerializeField]
		private Image Activator { get; set; }

		[field: SerializeField]
		private Material DefaultMaterial { get; set; }

		[field: SerializeField]
		private Material OnCooldownMaterial { get; set; }

		private Entity Ability { get; set; }

		public void Tick(Entity ability)
		{
			Ability = ability;

			var abilityInput = ECSUtils.GetSingleton<AbilityInput>();
			var entityManager = ECSUtils.ClientWorld.EntityManager;

			Activator.enabled = abilityInput.Ability == ability;

			var metaData = entityManager.GetComponentData<MetaData>(ability);
			var cooldown = entityManager.GetComponentData<Cooldown>(ability);
			var casting = entityManager.GetComponentData<Casting>(ability);

			Icon.sprite = metaData.GetIcon();
			Icon.material = cooldown.IsActive() ? OnCooldownMaterial : DefaultMaterial;

			Casting.Tick(casting);

			Cooldown.Tick(cooldown);
			Tooltip.Bind(metaData);
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
