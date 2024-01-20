using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Omniverse
{
	[UnityEngine.Scripting.Preserve]
	public class UnitManager: IFixedTickable, IPostFixedTickable, ITickable, IDisposable
	{
		private PrefabPool PrefabPool { get; }

		private ItemManager ItemManager { get; }

		private List<Unit> Units { get; } = new();

		private CancellationTokenSource CancellationTokenSource { get; } = new();
		
		public UnitManager(PrefabPool prefabPool, ItemManager itemManager)
		{
			PrefabPool = prefabPool;
			ItemManager = itemManager;
		}

		public void Dispose()
		{
			CancellationTokenSource.Dispose();
		}
		
		public Unit Spawn(UnitSpawnData data) => Spawn(data.UnitDesc, data.FactionID);

		public Unit Spawn(UnitDesc desc, int factionID)
		{
			var unit = new Unit(desc, factionID)
			{
				Presenter = PrefabPool.Take(desc.Presentation.Prefab)
			};

			unit.Presenter.Bind(unit);

			Units.Add(unit);

			return unit;
		}

		public void Despawn(Unit unit)
		{
			PrefabPool.Return(unit.Desc.Presentation.Prefab, unit.Presenter);
			Units.Remove(unit);
		}

		public void FixedTick()
		{
			for (var i = 0; i < Units.Count; i++)
			{
				Units[i].FixedTick();
			}
		}

		public void PostFixedTick()
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
				
				return unit.Resources.Values.Any(resource => resource.Vital && resource.OutOf);
			}
		}

		private void Kill(Unit unit)
		{
			unit.Die();
			DropLoot(unit);

			WaitForDespawn(unit, CancellationTokenSource.Token);
		}

		private async void WaitForDespawn(Unit unit, CancellationToken token)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(unit.Presenter.DespawnDelay), cancellationToken: token);
			PrefabPool.Return(unit.Desc.Presentation.Prefab, unit.Presenter);
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

				ItemManager.Spawn(loot.Item, unit.Presenter.transform.position, Quaternion.identity, null);
			}
		}

		public void Tick()
		{
			for (var i = 0; i < Units.Count; i++)
			{
				Units[i].Tick();
			}
		}

		public void Clear()
		{
			Units.Clear();
		}
	}
}
