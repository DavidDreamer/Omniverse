using System.Collections.Generic;
using System.Linq;
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

		public void Tick(EntityManager entityManager, Entity entity)
		{
			var abilityReferences = entityManager.GetBuffer<AbilityReference>(entity);

			var abilities = Abilities().ToArray();

			UpdateSlotsCount(entityManager, abilities.Length);

			for (int i = 0; i < abilities.Length; ++i)
			{
				Entity ability = abilities[i];
				Slots[i].Tick(entityManager, ability);
			}

			IEnumerable<Entity> Abilities()
			{
				foreach (var reference in abilityReferences)
				{
					yield return reference.Entity;
				}
			}
		}

		private void UpdateSlotsCount(EntityManager entityManager, int count)
		{
			int slotsDelta = count - Slots.Count;

			var inputSystemData = entityManager.GetSingletonManaged<InputSystemData>();
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
