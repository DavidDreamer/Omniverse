using System.Collections.Generic;
using System.Linq;
using Omniverse.Input;
using Unity.Entities;
using Unity.Transforms;
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

			var children = entityManager.GetBuffer<Child>(entity);

			var abilities = Abilities().ToArray();

			UpdateSlotsCount(abilities.Length);

			for (int i = 0; i < abilities.Length; ++i)
			{
				Entity ability = abilities[i];
				Slots[i].Tick(ability);
			}

			IEnumerable<Entity> Abilities()
			{
				foreach (var child in children)
				{
					if (entityManager.HasComponent<Ability>(child.Value))
					{
						yield return child.Value;
					}
				}
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
