using System;
using System.Collections.Generic;
using Omniverse.Abilities;
using Omniverse.Items;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Input
{
	public class Detector
	{
		public Entity Target { get; private set; }

		private HashSet<Type> DetectableTypes { get; } = new();

		private FactiousFilter Filter { get; set; }

		[Inject]
		private PhysicsService PhysicsService { get; set; }

		public void SetDefaultDetectableType()
		{
			ClearFilter();
			AddToFilter<Unit>();
			AddToFilter<Item>();
			Filter = (FactiousFilter)~0;
		}

		public void ClearFilter() => DetectableTypes.Clear();

		public void SetupForAbility(Ability ability)
		{
			ClearFilter();

			var target = ability.Desc.Target;
			var targetType = target.Type;

			if (targetType.HasFlag(TargetType.ResourceSource))
			{
				AddToFilter<ResourceSource>();
			}

			if (targetType.HasFlag(TargetType.Unit))
			{
				AddToFilter<Unit>();
			}

			Filter = target.Filter;
		}

		public void SetFiler<TEntityType>() where TEntityType : Entity
		{
			ClearFilter();
			AddToFilter<TEntityType>();
		}

		public void AddToFilter<TEntityType>() where TEntityType : Entity
		{
			DetectableTypes.Add(typeof(TEntityType));
		}

		public void RemoveFromFilter<TEntityType>() where TEntityType : Entity
		{
			DetectableTypes.Remove(typeof(TEntityType));
		}

		public void Tick(Ray ray, IFactious source)
		{
			Target = null;

			var entity = PhysicsService.GetEntity(ray);

			if (entity == null)
			{
				return;
			}

			if (!DetectableTypes.Contains(entity.GetType()))
			{
				return;
			}

			if (!Filter.Match(source, entity))
			{
				return;
			}

			Target = entity;
		}
	}
}
