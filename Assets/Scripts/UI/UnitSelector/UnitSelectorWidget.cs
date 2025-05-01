using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class UnitSelectorWidget : Widget
	{
		[field: SerializeField]
		private Canvas Canvas { get; set; }

		[field: SerializeField]
		private UnitSelectorItem[] Items { get; set; }

		private void OnValidate()
		{
			Items = GetComponentsInChildren<UnitSelectorItem>();
		}

		public override void Tick()
		{
			var selection = EntityManager.GetSingleton<Selection>();
			var entities = selection.Entities;
			int selectedEntitiesCount = entities.Length;

			bool multipleUnitsSelected = selectedEntitiesCount > 1;

			Canvas.enabled = multipleUnitsSelected;

			if (multipleUnitsSelected is false)
			{
				return;
			}

			int i = 0;
			foreach (Entity entity in entities)
			{
				UnitSelectorItem item = Items[i];
				item.gameObject.SetActive(true);
				var metaData = EntityManager.GetComponentData<MetaData>(entity);
				item.Icon.sprite = metaData.GetIcon();
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
