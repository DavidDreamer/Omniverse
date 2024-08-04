using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.FogOfWar;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Omniverse.Units
{
	[UnityEngine.Scripting.Preserve]
	public class Manager : IFixedTickable, IPostFixedTickable, IDisposable
	{
		[Inject]
		private PrefabPool<Unit> Pool { get; set; }

		[Inject]
		private Items.Manager ItemManager { get; set; }

		[Inject]
		private UnitManagerConfig Config { get; set; }

		[Inject]
		private FogOfWar.Manager FogOfWarManager { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public List<Unit> Units { get; } = new();

		private CancellationTokenSource CancellationTokenSource { get; } = new();

		private Dictionary<Unit, UnitFogOfWarAgent> FogOfWarAgents { get; } = new();

		public void Dispose()
		{
			CancellationTokenSource.Dispose();
		}

		public Unit Spawn(UnitDesc desc, int factionID)
		{
			Unit unit = Pool.Take(Config.UnitPrefab);
			unit.Initialize(desc);
			unit.ChangeFaction(factionID);
			Units.Add(unit);

			GameObject model = UnityEngine.Object.Instantiate(desc.Model, unit.transform, false);
			ObjectResolver.InjectGameObject(model);
			var components = model.GetComponentsInChildren<EntityComponent<Unit>>();
			foreach (var component in components)
			{
				component.Initialize(unit);
			}

			var unitFogOfWarAgent = new UnitFogOfWarAgent(unit);
			FogOfWarAgents.Add(unit, unitFogOfWarAgent);
			FogOfWarManager.Register(unitFogOfWarAgent);

			return unit;
		}

		public void Despawn(Unit unit)
		{
			FogOfWarManager.Unregister(FogOfWarAgents[unit]);
			FogOfWarAgents.Remove(unit);

			Pool.Return(unit);
			Units.Remove(unit);
		}

		public void FixedTick()
		{
			for (var i = 0; i < Units.Count; i++)
			{
				Unit unit = Units[i];
				UnitFogOfWarAgent fogOfWarAgent = FogOfWarAgents[unit];

				//TODO
				CellVisibilityState cellVisibilityState =
					FogOfWarManager.CellsVisibilityPerFaction[0][fogOfWarAgent.CellIndex];

				unit.gameObject.SetActive(cellVisibilityState is CellVisibilityState.Visible);

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
