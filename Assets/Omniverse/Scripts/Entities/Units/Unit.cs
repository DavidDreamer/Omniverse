using System;
using System.Collections.Generic;
using System.Threading;
using Omniverse.Abilities;
using Omniverse.Entities.Items;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omniverse.Entities.Units
{
	[Serializable]
	public class Unit: Entity<UnitDesc>
	{
		public event Action Died;
		
		public List<Ability> Abilities { get; } = new();

		public List<Effect> Effects { get; } = new();

		public bool IsDead { get; private set; }

		public bool Locked { get; set; }

		public UnitPresenter Presenter { get; set; }

		private CancellationTokenSource DeathCancellationTokenSource { get; set; } = new();

		public CancellationToken DeathCancellationToken => DeathCancellationTokenSource.Token;

		public UnitStatus Status { get; private set; }

		public IEntity Target { get; set; }
		
		public Experience Experience { get; private set; }
		
		public Attack Attack { get; private set; }

		public Inventory Inventory { get; private set; }
		
		public Unit(UnitDesc desc, int factionID): base(desc, factionID)
		{
			Experience = new Experience(desc.Experience);
			
			foreach (PropertyDesc resourceDesc in Desc.Properties)
			{
				Properties.Add(resourceDesc.ID, new Property(resourceDesc));
			}

			foreach (AbilityDesc abilityDesc in Desc.Abilities)
			{
				var ability = new Ability(abilityDesc);
				Abilities.Add(ability);
			}

			Attack = new Attack(this);
			Inventory = new Inventory(desc.Inventory);
		}

		public void FixedTick()
		{
			if (IsDead)
			{
				return;
			}
			
			UpdateEffects(Time.fixedDeltaTime);
			
			foreach (var property in Properties)
			{
				property.Value.FixedTick();
			}

			if (Locked)
			{
				return;
			}

			if (Presenter.NavMeshAgent != null)
			{
				ProcessTarget();
			}

			Presenter.FixedTick();
		}

		private void ProcessTarget()
		{
			switch (Target)
			{
				case null:
					return;
				case Unit unit:
				{
					Presenter.NavMeshAgent.destination = unit.Presenter.transform.position;

					if (Target.IsEnemyFor(this))
					{
						if (Presenter.NavMeshAgent.remainingDistance <= Properties[PropertyID.AttackRange].Amount)
						{
							if (Time.time - Attack.lastTime > Properties[PropertyID.AttackSpeed].Amount)
							{
								Presenter.NavMeshAgent.isStopped = true;
								Attack.Perform(unit);
							}
						}
					}

					break;
				}
				case Item item:
				{
					Presenter.NavMeshAgent.destination = item.Presenter.transform.position;
					if (Presenter.NavMeshAgent.remainingDistance <= Properties[PropertyID.AttackRange].Amount)
					{
						Object.Destroy(item.Presenter.gameObject);
						Inventory.Add(item);
						Target = null;
					}

					break;
				}
			}
		}
		
		private void UpdateEffects(float deltaTime)
		{
			Status = UnitStatus.None;

			for (var i = 0; i < Effects.Count; ++i)
			{
				Effect effect = Effects[i];
				effect.Tick(deltaTime);

				if (effect.OutOfTime)
				{
					foreach (PropertyModifierDesc desc in effect.Desc.PropertyModifiers)
					{
						Properties[desc.ID].RemoveModifier(desc.Modifier);
					}
					Effects.RemoveAt(i);
					i--;
				}
				else
				{
					Status |= effect.Desc.UnitStatus;
				}
			}
		}

		public void ChangeResource(ChangePropertyData data)
		{
			Property resource = Properties[data.ID];

			resource.Change(data.Amount);
		}

		public void ApplyEffect(Effect effect)
		{
			Effects.Add(effect);

			foreach (PropertyModifierDesc desc in effect.Desc.PropertyModifiers)
			{
				Properties[desc.ID].AddModifier(desc.Modifier);
			}
		}

		internal void Die()
		{
			IsDead = true;

			DeathCancellationTokenSource.Cancel();
			DeathCancellationTokenSource.Dispose();
			DeathCancellationTokenSource = null;

			Presenter.OnDeath();
			
			Died?.Invoke();
		}
	}
}
