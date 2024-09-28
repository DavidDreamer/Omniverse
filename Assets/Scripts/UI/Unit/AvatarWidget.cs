using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Omniverse.UI
{
	public class AvatarWidget : MonoBehaviour, IPointerClickHandler
	{
		[Inject]
		public CameraController CameraController { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					Unit selectedUnit = Selector.SelectedUnit;
					Vector3 selectedUnitPosition = selectedUnit.transform.position;
					var viewPoint = new Vector3(selectedUnitPosition.x, 0, selectedUnitPosition.z);
					CameraController.SetViewPoint(viewPoint);
					break;
			}
		}
	}
}
