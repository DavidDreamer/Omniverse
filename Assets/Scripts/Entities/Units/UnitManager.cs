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
		private PrefabPool<UnitObsolete> Pool { get; set; }

		[Inject]
		private ItemManager ItemManager { get; set; }

		[Inject]
		private UnitManagerConfig Config { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public List<UnitObsolete> Units { get; } = new();

		public UnitObsolete Spawn(UnitDesc desc, int factionID, Vector3 position)
		{
			UnitObsolete unit = Pool.Take(Config.UnitPrefab);
			unit.NavMeshAgent.Warp(position);
			unit.Initialize(desc, factionID);
			Units.Add(unit);

			GameObject model = Object.Instantiate(desc.Model, unit.transform, false);
			ObjectResolver.InjectGameObject(model);
			var components = model.GetComponentsInChildren<OmniverseEntityComponent<UnitObsolete>>();
			foreach (var component in components)
			{
				component.Initialize(unit);
			}

			return unit;
		}

		public void Despawn(UnitObsolete unit)
		{
			Pool.Return(unit);
			Units.Remove(unit);
		}

		public void Tick(float deltaTime)
		{
			for (var i = 0; i < Units.Count; i++)
			{
				UnitObsolete unit = Units[i];
				Units[i].FixedTick(deltaTime);
			}
		}

		public void UpdateLivingState()
		{
			for (var i = 0; i < Units.Count; i++)
			{
				UnitObsolete unit = Units[i];

				if (ShouldBeKilled(unit))
				{
					Kill(unit);
					Units.RemoveAt(i);
					i--;
				}
			}

			bool ShouldBeKilled(UnitObsolete unit)
			{
				if (unit.IsDead)
				{
					return false;
				}

				return unit.Properties.Values.Any(resource => resource.Vital && resource.OutOf);
			}
		}

		private void Kill(UnitObsolete unit)
		{
			unit.Die();
			DropLoot(unit);

			//TODO: Add non-async delay
			Pool.Return(unit);
		}

		private void DropLoot(UnitObsolete unit)
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
