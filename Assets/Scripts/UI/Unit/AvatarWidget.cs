using Omniverse.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class AvatarWidget : MonoBehaviour, IPointerClickHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		public void Bind(UnitObsolete unit)
		{
			Icon.sprite = unit.Desc.Icon;
			Name.text = unit.Desc.Name;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					UnitObsolete selectedUnit = Selector.SelectedUnit;
					Vector3 selectedUnitPosition = selectedUnit.transform.position;
					var viewPoint = new Vector3(selectedUnitPosition.x, 0, selectedUnitPosition.z);
					Object.FindFirstObjectByType<CameraController>().SetViewPoint(viewPoint);
					break;
			}
		}
	}
}
