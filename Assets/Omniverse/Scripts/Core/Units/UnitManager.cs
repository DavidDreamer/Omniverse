using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Omniverse
{
	[UnityEngine.Scripting.Preserve]
	public class UnitManager: IFixedTickable, IPostFixedTickable, IDisposable
	{
		[Inject]
		private PrefabPool<UnitPresenter> PrefabPool { get; set; }

		[Inject]
		private ItemManager ItemManager { get; set; }

		[Inject]
		private UnitManagerConfig Config { get; set; }
		
		private List<Unit> Units { get; } = new();

		private CancellationTokenSource CancellationTokenSource { get; } = new();

		public void Dispose()
		{
			CancellationTokenSource.Dispose();
		}
		
		public Unit Spawn(UnitSpawnData data) => Spawn(data.UnitDesc, data.FactionID);

		public Unit Spawn(UnitDesc desc, int factionID)
		{
			UnitPresenter unitPresenter = Object.Instantiate(Config.UnitPresenter);
			
			var unit = new Unit(desc, factionID)
			{
				Presenter = unitPresenter
			};

			unit.Presenter.Bind(unit);

			UnitRenderer unitRenderer = Object.Instantiate(desc.Presentation.Prefab, unitPresenter.transform);
			unitRenderer.Initialize(unit);
	
			Units.Add(unit);

			return unit;
		}

		public void Despawn(Unit unit)
		{
			PrefabPool.Return(unit.Presenter);
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
				
				return unit.Properties.Values.Any(resource => resource.Vital && resource.OutOf);
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
			await UniTask.Delay(TimeSpan.FromSeconds(Config.DespawnDelay), cancellationToken: token);
			PrefabPool.Return(unit.Presenter);
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

				ItemManager.Spawn(loot.Item, unit.Presenter.transform.position, Quaternion.identity);
			}
		}

		public void Clear()
		{
			Units.Clear();
		}
	}
}
