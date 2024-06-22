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
	//TODO:
	public abstract class UnitRendererBase: MonoBehaviour, IPoolObject
	{
		public abstract void Initialize(Unit unit);

		public abstract void Cleanup();
	}

	[UnityEngine.Scripting.Preserve]
	public class Manager: IFixedTickable, IPostFixedTickable, IDisposable
	{
		[Inject]
		private PrefabPool<UnitPresenter> PresenterPool { get; set; }

		[Inject]
		private PrefabPool<UnitRendererBase> RendererPool { get; set; }

		[Inject]
		private Items.Manager ItemManager { get; set; }

		[Inject]
		private UnitManagerConfig Config { get; set; }

		[Inject]
		private FogOfWar.Manager FogOfWarManager { get; set; }

		private List<Unit> Units { get; } = new();

		private CancellationTokenSource CancellationTokenSource { get; } = new();

		private Dictionary<Unit, UnitFogOfWarAgent> FogOfWarAgents { get; } = new();

		public void Dispose()
		{
			CancellationTokenSource.Dispose();
		}
		
		public Unit Spawn(UnitDesc desc, int factionID)
		{
			UnitPresenter unitPresenter = PresenterPool.Take(Config.UnitPresenter);

			var unit = new Unit(desc, factionID)
			{
				Presenter = unitPresenter
			};

			unitPresenter.Bind(unit);

			UnitRendererBase unitRenderer = RendererPool.Take(desc.Presentation.Prefab);
			unitRenderer.transform.SetParent(unitPresenter.transform, false);
			unitRenderer.Initialize(unit);

			Units.Add(unit);

			var unitFogOfWarAgent = new UnitFogOfWarAgent(unit);
			FogOfWarAgents.Add(unit, unitFogOfWarAgent);
			FogOfWarManager.Register(unitFogOfWarAgent);

			return unit;
		}

		public void Despawn(Unit unit)
		{
			FogOfWarManager.Unregister(FogOfWarAgents[unit]);
			FogOfWarAgents.Remove(unit);

			PresenterPool.Return(unit.Presenter);
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
				
				unit.Presenter.GetComponentInChildren<UnitRendererBase>(true).gameObject
					.SetActive(cellVisibilityState is CellVisibilityState.Visible);

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
			PresenterPool.Return(unit.Presenter);
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
