using System;
using System.Collections.Generic;
using System.Threading;
using Omniverse.Abilities;
using Omniverse.Entities.Items;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Object = UnityEngine.Object;

namespace Omniverse.Entities.Units
{
	public class Unit: Entity<UnitDesc>, IPoolObject
	{
		public event Action Died;
		
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }
		
		public List<Ability> Abilities { get; } = new();

		public List<Effect> Effects { get; } = new();

		public bool IsDead { get; private set; }

		public bool Locked { get; set; }
		
		private CancellationTokenSource DeathCancellationTokenSource { get; set; } = new();

		public CancellationToken DeathCancellationToken => DeathCancellationTokenSource.Token;

		public UnitStatus Status { get; private set; }

		public IEntity Target { get; set; }
		
		public Experience Experience { get; private set; }
		
		public Attack Attack { get; private set; }

		public Inventory Inventory { get; private set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }
		
		public override void Initialize(UnitDesc desc, int factionID)
		{
			base.Initialize(desc, factionID);
			
			Experience = new Experience(desc.Experience);
			
			foreach (PropertyDesc resourceDesc in Desc.Properties)
			{
				Properties.Add(resourceDesc.ID, new Property(resourceDesc));
			}

			foreach (AbilityDesc abilityDesc in Desc.Abilities)
			{
				var ability = new Ability(ObjectResolver, abilityDesc);
				Abilities.Add(ability);
			}

			Attack = new Attack(this);
			Inventory = new Inventory(desc.Inventory);
		}

		public void Cleanup()
		{
			if (HitBox != null)
			{
				HitBox.enabled = true;
			}

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = true;
			}
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
			
			if (NavMeshAgent != null)
			{
				ProcessTarget();
				
				if (Properties.TryGetValue(PropertyID.MovementSpeed, out Property property))
				{
					NavMeshAgent.speed = property.Amount;
				}
				else
				{
					NavMeshAgent.speed = 0;
				}
				
				if (Properties.TryGetValue(PropertyID.RotationSpeed, out property))
				{
					NavMeshAgent.angularSpeed = property.Amount;
				}
				else
				{
					NavMeshAgent.angularSpeed = 0;
				}
				
				NavMeshAgent.isStopped = Status.HasFlag(UnitStatus.Stunned);
			}
		}

		private void ProcessTarget()
		{
			switch (Target)
			{
				case null:
					return;
				case Unit unit:
				{
					NavMeshAgent.destination = unit.transform.position;

					if (Target.IsEnemyFor(this))
					{
						if (NavMeshAgent.remainingDistance <= Properties[PropertyID.AttackRange].Amount)
						{
							if (Time.time - Attack.lastTime > Properties[PropertyID.AttackSpeed].Amount)
							{
								NavMeshAgent.isStopped = true;
								Attack.Perform(unit);
							}
						}
					}

					break;
				}
				case Item item:
				{
					NavMeshAgent.destination = item.transform.position;
					if (NavMeshAgent.remainingDistance <= Properties[PropertyID.AttackRange].Amount)
					{
						Object.Destroy(item.gameObject);
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

			HitBox.enabled = false;

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = false;
			}
			
			Died?.Invoke();
		}
	}
}
