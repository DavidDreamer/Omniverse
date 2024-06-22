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
		public List<IEntityPresenter> Entities { get; } = new();

		[Inject]
		public OutlineRendererFeature Outline { get; private set; }
		
		[Inject]
		private IPlayer Player { get; set; }

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
				var entityPresenter = hitInfo.collider.GetComponent<IEntityPresenter>();
				if (entityPresenter != null)
				{
					AddFocus(entityPresenter);
				}
			}
		}
		
		private void AddFocus(IEntityPresenter entityPresenter)
		{
			Entities.Add(entityPresenter);

			foreach (Renderer renderer in entityPresenter.Renderers)
			{
				int variant = GetOutlineVariantByFactionID(entityPresenter.Entity.FactionID);
				var outlineRenderer = new OutlineRenderer(renderer, variant);
				Outline.Pass.AddRenderer(outlineRenderer);
			}
		}

		private int GetOutlineVariantByFactionID(int factionID)
		{
			if (factionID == -1)
			{
				return 2;
			}

			return factionID == Player.FactionID ? 0 : 1;
		}
		
		private void Clear()
		{
			Entities.Clear();
			Outline.Pass.Clear();
		}
	}
}
