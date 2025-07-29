using Omniverse.Input;
using TMPro;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class AvatarWidget : Widget, IPointerClickHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		public void Bind(Entity entity)
		{
			if (EntityManager.HasComponent<MetaData>(entity))
			{
				var metaData = EntityManager.GetComponentData<MetaData>(entity);
				Icon.sprite = metaData.Icon;
				Name.text = metaData.Name.ToString();
			}
			else
			{
				Icon.sprite = null;
				Name.text = string.Empty;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					var selection = EntityManager.GetSingleton<Selection>();
					Entity selectedUnit = selection.Entity;
					Vector3 selectedUnitPosition = EntityManager.GetComponentData<LocalToWorld>(selectedUnit).Position;
					var viewPoint = new Vector3(selectedUnitPosition.x, 0, selectedUnitPosition.z);
					var system = EntityManager.World.GetExistingSystemManaged<ProcessCameraInputSystem>();
					system.SetViewPoint(viewPoint);
					break;
			}
		}
	}
}
