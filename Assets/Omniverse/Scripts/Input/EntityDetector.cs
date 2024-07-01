using System;
using System.Collections.Generic;
using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class EntityDetector: ILateTickable
	{
		public Entity Target { get; private set; }

		[Inject]
		private Player Player { get; set; }

		private HashSet<Type> DetectableTypes { get; } = new();

		public EntityDetector()
		{
			SetDefaultDetectableType();
		}

		public void SetDefaultDetectableType() => SetFiler<Unit>();

		public void ClearFilter() => DetectableTypes.Clear();
		
		public void SetFiler<TEntityType>() where TEntityType: Entity
		{
			ClearFilter();
			AddToFilter<TEntityType>();
		}

		public void AddToFilter<TEntityType>() where TEntityType: Entity
		{
			DetectableTypes.Add(typeof(TEntityType));
		}

		public void RemoveFromFilter<TEntityType>() where TEntityType: Entity
		{
			DetectableTypes.Remove(typeof(TEntityType));
		}

		public void LateTick()
		{
			Target = null;

			Camera camera = Camera.main;

			if (camera == null)
			{
				return;
			}

			Vector2 mousePosition = Mouse.current.position.value;
			Ray ray = camera.ScreenPointToRay(mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue))
			{
				var entityPresenter = hitInfo.collider.GetComponent<Entity>();
				if (entityPresenter != null && DetectableTypes.Contains(entityPresenter.GetType()))
				{
					Target = entityPresenter;
				}
			}
		}
	}
}
