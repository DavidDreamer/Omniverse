using System;
using System.Collections.Generic;
using Dreambox.Rendering.URP;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class EntityDetector: ILateTickable
	{
		public List<EntityPresenter> Entities { get; } = new();

		[Inject]
		public OutlineRendererFeature Outline { get; private set; }
		
		// [Inject]
		// private Player Player { get; set; }
		//
		
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

			var renderers = entityPresenter.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				Outline.Pass.AddRenderer(renderer);
			}
		}
		
		private void Clear()
		{
			Entities.Clear();
			Outline.Pass.Clear();
		}
	}
}
