using System.Collections.Generic;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.UI
{
	public class AbilityBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private AbilitySlotWidget AbilitySlotWidgetPrefab { get; set; }

		[field: SerializeField]
		private RectTransform Holder { get; set; }

		private List<AbilitySlotWidget> Slots { get; } = new();

		public void Tick(Entity entity)
		{
			EntityManager entityManager = ECSUtils.ClientWorld.EntityManager;
			var abilityModule = entityManager.GetComponentObject<AbilityModule>(entity);
			var abilities = abilityModule.Abilities;

			int count = abilities.Count;
			UpdateSlotsCount(count);

			for (int i = 0; i < count; ++i)
			{
				Ability ability = abilities[i];
				Slots[i].Tick(ability);
			}
		}

		private void UpdateSlotsCount(int count)
		{
			int slotsDelta = count - Slots.Count;

			var inputSystemData = ECSUtils.GetSingletonManaged<InputSystemData>();
			var abilityActions = inputSystemData.InputActions.Abilities;

			if (slotsDelta > 0)
			{
				while (slotsDelta != 0)
				{
					AbilitySlotWidget slot = Instantiate(AbilitySlotWidgetPrefab, Holder);
					int slotIndex = Slots.Count;
					InputAction inputAction = abilityActions.Get().actions[slotIndex];
					slot.Initialize(inputAction);
					Slots.Add(slot);
					slotsDelta--;
				}
			}
			else if (slotsDelta < 0)
			{
				while (slotsDelta != 0)
				{
					int indexToRemove = Slots.Count - 1;
					Destroy(Slots[indexToRemove].gameObject);
					Slots.RemoveAt(indexToRemove);
					slotsDelta++;
				}
			}

			Holder.ForceLayoutRebuild();
		}
	}
}
