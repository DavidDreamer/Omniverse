using System.Collections.Generic;
using Dreambox.Rendering.URP;
using Omniverse.Entities.Units;
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
		
		[Inject]
		private Player Player { get; set; }

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

			foreach (Renderer renderer in entityPresenter.Renderers)
			{
				int variant = GetOutlineVariantByFactionID(entityPresenter);
				var outlineRenderer = new OutlineRenderer(renderer, variant);
				Outline.Pass.AddRenderer(outlineRenderer);
			}
		}

		private int GetOutlineVariantByFactionID(EntityPresenter entityPresenter)
		{
			switch (entityPresenter)
			{
				case UnitPresenter unitPresenter:
					return unitPresenter.Entity.FactionID == Player.FactionID ? 0 : 1;
				default:
					return 2;
			}
		}
		
		private void Clear()
		{
			Entities.Clear();
			Outline.Pass.Clear();
		}
	}
}
