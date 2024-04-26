using System;
using System.Collections.Generic;
using System.Threading;
using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class Unit
	{
		public event Action Died;
		
		public UnitDesc Desc { get; }

		public int FactionID { get; set; }

		public Dictionary<PropertyTag, Property> Properties { get; }

		public List<Ability> Abilities { get; } = new();

		public List<Effect> Effects { get; } = new();

		public bool IsDead { get; private set; }

		public bool Locked { get; set; }

		public UnitPresenter Presenter { get; set; }

		private CancellationTokenSource DeathCancellationTokenSource { get; set; } = new();

		public CancellationToken DeathCancellationToken => DeathCancellationTokenSource.Token;

		public UnitStatus Status { get; private set; }

		public Unit Target { get; set; }
		
		public Movement Movement { get; private set; }
		
		public Attack Attack { get; private set; }
		
		public Unit(UnitDesc desc, int factionID)
		{
			Desc = desc;
			FactionID = factionID;
			
			Properties = new Dictionary<PropertyTag, Property>();
			foreach (PropertyDesc resourceDesc in Desc.Properties)
			{
				Properties.Add(resourceDesc.Tag, new Property(resourceDesc));
			}

			foreach (AbilityDesc abilityDesc in Desc.Abilities)
			{
				var ability = new Ability(abilityDesc, this);
				Abilities.Add(ability);
			}

			if (desc.Movement != null)
			{
				Movement = new Movement(desc.Movement);
			}
			
			Attack = new Attack(desc.Attack);
		}

		public void FixedTick()
		{
			foreach (var resource in Properties)
			{
				resource.Value.FixedTick();
			}

			UpdateEffects(Time.fixedDeltaTime);
			
			if (IsDead)
			{
				return;
			}

			if (Locked)
			{
				return;
			}

			if (Presenter.NavMeshAgent != null)
			{
				if (Target != null)
				{
					Presenter.NavMeshAgent.destination = Target.Presenter.transform.position;

					if (Target.IsEnemyFor(this))
					{
						if (Presenter.NavMeshAgent.remainingDistance <= Attack.Range)
						{
							if (Time.time - Attack.lastTime > Attack.Speed)
							{
								Presenter.NavMeshAgent.isStopped = true;
								Attack.Perform(Target);
							}
						}
					}
				}
			}

			Presenter.FixedTick();
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
			Property resource = Properties[data.Tag];

			resource.Change(data.Amount);
		}

		public void ApplyEffect(Effect effect)
		{
			Effects.Add(effect);
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
