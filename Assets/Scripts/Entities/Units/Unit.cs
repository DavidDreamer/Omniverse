using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Items;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse.Units
{
	public class Unit : FactiousEntity<UnitDesc>, IPoolObject
	{
		public event Action<Effect> EffectApplied;

		public event Action<Effect> EffectRemoved;

		public event Action Died;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		public Dictionary<PropertyID, Property> Properties { get; } = new();

		public List<Ability> Abilities { get; } = new();

		public List<Effect> Effects { get; } = new();

		public bool IsDead { get; private set; }

		public bool Locked { get; set; }

		private CancellationTokenSource DeathCancellationTokenSource { get; set; } = new();

		public CancellationToken DeathCancellationToken => DeathCancellationTokenSource.Token;

		public UnitStatus Status { get; private set; }

		public Entity Target { get; set; }

		public Experience Experience { get; private set; }

		public Attack Attack { get; private set; }

		public Inventory Inventory { get; private set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public override void Initialize(UnitDesc desc)
		{
			base.Initialize(desc);

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

				//NavMeshAgent.isStopped = Status.HasFlag(UnitStatus.Stunned);
			}
		}

		public void Stop()
		{
			NavMeshAgent.isStopped = true;
		}

		public void Start()
		{
			NavMeshAgent.isStopped = false;
		}

		public void MoveToPosition(Vector3 position)
		{
			Target = null;
			Start();
			NavMeshAgent.destination = position;
		}

		private void ProcessTarget()
		{
			switch (Target)
			{
				case null:
					return;
				case Unit unit:
					{
						if (Attack.CanAttack(unit))
						{
							Stop();
							Attack.Perform(unit, default).Forget();
						}
						else if (!Attack.InProcess)
						{
							Start();
							NavMeshAgent.destination = unit.transform.position;
						}

						break;
					}
				case Item item:
					{
						NavMeshAgent.destination = item.transform.position;
						if (NavMeshAgent.remainingDistance <= Properties[PropertyID.AttackRange].Amount)
						{
							Destroy(item.gameObject);
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
					RemoveEffect(effect, i);
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

			EffectApplied?.Invoke(effect);
		}

		public void RemoveEffect(Effect effect, int index)
		{
			foreach (PropertyModifierDesc desc in effect.Desc.PropertyModifiers)
			{
				Properties[desc.ID].RemoveModifier(desc.Modifier);
			}

			Effects.RemoveAt(index);

			EffectRemoved?.Invoke(effect);
		}

		public async UniTaskVoid Cast(Ability ability, CancellationToken token)
		{
			ability.InProcess = true;

			//TODO
			// if (!string.IsNullOrEmpty(Desc.Cast.AnimationTrigger))
			// {
			// 	Unit.Presenter.Animator.SetTrigger(AnimatorParameter.Get(Desc.Cast.AnimationTrigger));
			// }

			await UniTask.Delay(TimeSpan.FromSeconds(ability.Desc.Cast.Time), cancellationToken: token);

			ability.InProcess = false;

			foreach (CostDesc cost in ability.Desc.Cost)
			{
				Properties[cost.PropertyID].Change(-cost.Amount);
			}

			await ability.Cast(this, token);
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
