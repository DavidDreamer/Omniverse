using Omniverse.Cameras;
using Omniverse.Entities.Units;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Omniverse.UI
{
	public class AvatarWidget: MonoBehaviour, IPointerClickHandler
	{
		[Inject]
		public CameraController CameraController { get; set; }
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }
		
		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					Unit selectedUnit = UnitSelector.SelectedUnit.Unit;
					Vector3 selectedUnitPosition = selectedUnit.Presenter.transform.position;
					var viewPoint = new Vector3(selectedUnitPosition.x, 0, selectedUnitPosition.z);
					CameraController.SetViewPoint(viewPoint);
					break;
			}
		}
	}
}
