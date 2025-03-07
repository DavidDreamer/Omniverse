using System.Collections.Generic;
using System.Linq;
using Omniverse.Items;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Omniverse
{
	public class UnitManager
	{
		[Inject]
		private PrefabPool<Unit> Pool { get; set; }

		[Inject]
		private ItemManager ItemManager { get; set; }

		[Inject]
		private UnitManagerConfig Config { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public List<Unit> Units { get; } = new();

		public Unit Spawn(UnitDesc desc, int factionID, Vector3 position)
		{
			Unit unit = Pool.Take(Config.UnitPrefab);
			unit.NavMeshAgent.Warp(position);
			unit.Initialize(desc, factionID);
			Units.Add(unit);

			GameObject model = Object.Instantiate(desc.Model, unit.transform, false);
			ObjectResolver.InjectGameObject(model);
			var components = model.GetComponentsInChildren<OmniverseEntityComponent<Unit>>();
			foreach (var component in components)
			{
				component.Initialize(unit);
			}

			return unit;
		}

		public void Despawn(Unit unit)
		{
			Pool.Return(unit);
			Units.Remove(unit);
		}

		public void Tick(float deltaTime)
		{
			for (var i = 0; i < Units.Count; i++)
			{
				Unit unit = Units[i];
				Units[i].FixedTick(deltaTime);
			}
		}

		public void UpdateLivingState()
		{
			for (var i = 0; i < Units.Count; i++)
			{
				Unit unit = Units[i];

				if (ShouldBeKilled(unit))
				{
					Kill(unit);
					Units.RemoveAt(i);
					i--;
				}
			}

			bool ShouldBeKilled(Unit unit)
			{
				if (unit.IsDead)
				{
					return false;
				}

				return unit.Properties.Values.Any(resource => resource.Vital && resource.OutOf);
			}
		}

		private void Kill(Unit unit)
		{
			unit.Die();
			DropLoot(unit);

			//TODO: Add non-async delay
			Pool.Return(unit);
		}

		private void DropLoot(Unit unit)
		{
			foreach (LootDesc loot in unit.Desc.Loot)
			{
				float random = Random.Range(0f, 1f);

				if (loot.DropChance <= random)
				{
					continue;
				}

				ItemManager.Spawn(loot.Item, unit.transform.position, Quaternion.identity);
			}
		}

		public void Clear()
		{
			Units.Clear();
		}
	}
}
