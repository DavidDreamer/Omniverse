using System.Collections.Generic;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class AbilityBarWidget: MonoBehaviour
	{
		[field: SerializeField]
		private AbilitySlotWidget AbilitySlotWidgetPrefab { get; set; }
		
		[field: SerializeField]
		private RectTransform Holder { get; set; }

		private List<AbilitySlotWidget> Slots { get; } = new();
		
		private Unit Unit { get; set; }
		
		[Inject]
		private InputActions.AbilitiesActions InputActions { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }
		
		public void Bind(Unit unit)
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

			if (slotsDelta > 0)
			{
				while (slotsDelta != 0)
				{
					AbilitySlotWidget slot = ObjectResolver.Instantiate(AbilitySlotWidgetPrefab, Holder);
					int slotIndex = Slots.Count;
					InputAction inputAction = InputActions.Get().actions[slotIndex];
					slot.Initialize(inputAction);
					Slots.Add(slot);
					slotsDelta--;
				}
			}
			else if (slotsDelta < 0)
			{
				while (slotsDelta != 0)
				{
					Slots.RemoveAt(0);
					Destroy(Slots[0].gameObject);
					slotsDelta++;
				}
			}

			Holder.ForceLayoutRebuild();
		}
	}
}
