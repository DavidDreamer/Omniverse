using System;
using System.Collections.Generic;
using Omniverse.Abilities;
using Omniverse.Items;
using UnityEngine;

namespace Omniverse.Input
{
	public class Detector
	{
		public OmniverseEntity Target { get; private set; }

		private HashSet<Type> DetectableTypes { get; } = new();

		private FactiousFilter Filter { get; set; }

		public void SetDefaultDetectableType()
		{
			ClearFilter();
			AddToFilter<UnitObsolete>();
			AddToFilter<Item>();
			Filter = (FactiousFilter)~0;
		}

		public void ClearFilter() => DetectableTypes.Clear();

		public void SetupForAbility(Ability ability)
		{
			ClearFilter();

			switch (ability.Desc.Target)
			{
				case UnitTarget unitTarget:
					AddToFilter<UnitObsolete>();
					Filter = unitTarget.Filter;
					break;
				case ResourceSourceTarget resourceSourceTarget:
					AddToFilter<ResourceSource>();
					Filter = resourceSourceTarget.Filter;
					break;
			}
		}

		public void SetFiler<TEntityType>() where TEntityType : OmniverseEntity
		{
			ClearFilter();
			AddToFilter<TEntityType>();
		}

		public void AddToFilter<TEntityType>() where TEntityType : OmniverseEntity
		{
			DetectableTypes.Add(typeof(TEntityType));
		}

		public void RemoveFromFilter<TEntityType>() where TEntityType : OmniverseEntity
		{
			DetectableTypes.Remove(typeof(TEntityType));
		}

		public void Tick(Ray ray, int sourceFactionID)
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

			if (!Filter.Match(sourceFactionID, entity.FactionID))
			{
				return;
			}

			Target = entity;
		}
	}
}
