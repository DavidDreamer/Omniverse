using Omniverse.Input;
using TMPro;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class AvatarWidget : MonoBehaviour, IPointerClickHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		public void Bind(Entity entity)
		{
			var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var metaData = entityManager.GetComponentData<MetaData>(entity);

			Icon.sprite = metaData.GetIcon();
			Name.text = metaData.Name.ToString();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					var selection = ECSUtils.GetSingleton<Selection>();
					Entity selectedUnit = selection.Entity;
					var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
					Vector3 selectedUnitPosition = entityManager.GetComponentData<LocalToWorld>(selectedUnit).Position;
					var viewPoint = new Vector3(selectedUnitPosition.x, 0, selectedUnitPosition.z);
					Object.FindFirstObjectByType<CameraController>().SetViewPoint(viewPoint);
					break;
			}
		}
	}
}
