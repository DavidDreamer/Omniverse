using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UnitSelectorWidget: MonoBehaviour, ILateTickable
	{
		[field: SerializeField]
		private Canvas Canvas { get; set; }
		
		[field: SerializeField]
		private UnitSelectorItem[] Items { get; set; }
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }

		private void OnValidate()
		{
			Items = GetComponentsInChildren<UnitSelectorItem>();
		}

		public void LateTick()
		{
			var selectedUnits = UnitSelector.SelectedUnits;
			int selectedUnitsCount = selectedUnits.Count;

			bool multipleUnitsSelected = selectedUnitsCount > 1;

			Canvas.enabled = multipleUnitsSelected;

			if (multipleUnitsSelected is false)
			{
				return;
			}

			int i = 0;
			while (i < selectedUnitsCount)
			{
				UnitSelectorItem item = Items[i];
				item.gameObject.SetActive(true);
				item.Icon.sprite = selectedUnits[i].Desc.Icon;
				i++;
			}

			while (i < Items.Length)
			{
				Items[i].gameObject.SetActive(false);
				i++;
			}
		}
	}
}
