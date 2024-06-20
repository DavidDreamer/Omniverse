using System.Collections.Generic;
using Omniverse.Units.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class EntityDetector: ILateTickable
	{
		public List<EntityPresenter> Entities { get; } = new();

		public void LateTick()
		{
			Clear();
			
			Camera camera = Camera.main;

			if (camera == null)
			{
				return;
			}
			
			Vector2 mousePosition = Mouse.current.position.value;
			Ray ray = camera.ScreenPointToRay(mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue))
			{
				var entityPresenter = hitInfo.collider.GetComponent<EntityPresenter>();
				if (entityPresenter != null)
				{
					AddFocus(entityPresenter);
				}
			}
		}
		
		private void AddFocus(EntityPresenter entityPresenter)
		{
			Entities.Add(entityPresenter);
		}
		
		private void Clear()
		{
			Entities.Clear();
		}
	}
}
