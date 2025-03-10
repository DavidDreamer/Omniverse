using System.Collections.Generic;
using Omniverse.Abilities;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class AbilityBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private AbilitySlotWidget AbilitySlotWidgetPrefab { get; set; }

		[field: SerializeField]
		private RectTransform Holder { get; set; }

		private List<AbilitySlotWidget> Slots { get; } = new();

		private UnitObsolete Unit { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public void Bind(UnitObsolete unit)
		{
			Unit = unit;

			int count = unit.Abilities.Count;
			UpdateSlotsCount(unit.Desc.Abilities.Count);

			for (int i = 0; i < count; ++i)
			{
				Ability ability = unit.Abilities[i];
				Slots[i].Bind(ability);
			}
		}

		public void Tick()
		{
			foreach (AbilitySlotWidget slot in Slots)
			{
				slot.Tick();
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
					AbilitySlotWidget slot = ObjectResolver.Instantiate(AbilitySlotWidgetPrefab, Holder);
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
