using System;
using System.Collections.Generic;
using System.Threading;
using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse.Units
{
	public class Unit : Entity<UnitDesc>, IPoolObject
	{
		public event Action<Effect> EffectApplied;

		public event Action<Effect> EffectRemoved;

		public event System.Action Died;

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		public Dictionary<PropertyID, Property> Properties { get; } = new();

		public List<Ability> Abilities { get; } = new();

		public List<Effect> Effects { get; } = new();

		public bool IsDead { get; private set; }

		private CancellationTokenSource DeathCancellationTokenSource { get; set; } = new();

		public CancellationToken DeathCancellationToken => DeathCancellationTokenSource.Token;

		public UnitStatus Status { get; private set; }

		public Experience Experience { get; private set; }

		public Attack Attack { get; private set; }

		public Inventory Inventory { get; private set; }

		public CommandModule CommandModule { get; private set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		[Inject]
		private ResourceExtractionHadler ResourceExtractionHadler { get; set; }

		[Inject]
		private TempNameManager TempNameManager { get; set; }

		[Inject]
		public PhysicsService PhysicsService { get; set; }

		public void Initialize(UnitDesc desc, int factionID)
		{
			base.Initialize(desc);

			Experience = new Experience(desc.Experience);

			foreach (PropertyDesc resourceDesc in Desc.Properties)
			{
				Properties.Add(resourceDesc.ID, new Property(resourceDesc));
			}

			foreach (AbilityDesc abilityDesc in Desc.Abilities)
			{
				var ability = new Ability(abilityDesc, this);
				Abilities.Add(ability);
			}

			Attack = new Attack(this);
			Inventory = new Inventory(desc.Inventory);
			CommandModule = new CommandModule(this);

			ChangeFaction(factionID);
		}

		public void Cleanup()
		{
			if (HitBox != null)
			{
				HitBox.enabled = true;
			}

			NavMeshAgent.enabled = true;
		}

		public void FixedTick(float deltaTime)
		{
			if (IsDead)
			{
				return;
			}

			UpdateAbilities();
			UpdateEffects();
			UpdateProperties();
			CommandModule.Tick(deltaTime);

			//NavMeshAgent.isStopped = Status.HasFlag(UnitStatus.Stunned);

			void UpdateAbilities()
			{
				foreach (Ability ability in Abilities)
				{
					ability.Tick(deltaTime);
				}
			}

			void UpdateEffects()
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

			void UpdateProperties()
			{
				if (Properties.TryGetValue(PropertyID.MovementSpeed, out Property movementSpeed))
				{
					NavMeshAgent.speed = movementSpeed.Amount;
				}
				else
				{
					NavMeshAgent.speed = 0;
				}

				if (Properties.TryGetValue(PropertyID.RotationSpeed, out Property rotationSpeed))
				{
					NavMeshAgent.angularSpeed = rotationSpeed.Amount;
				}
				else
				{
					NavMeshAgent.angularSpeed = 0;
				}

				foreach (var property in Properties)
				{
					property.Value.FixedTick(deltaTime);
				}
			}
		}

		public void ModifyProperty(PropertyID propertyID, PropertyModifier modifier, Entity source)
		{
			Property property = Properties[propertyID];
			property.Modify(modifier);
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

			if (effect.Desc.OnRemovedAction != null)
			{
				//TODO
				effect.Desc.OnRemovedAction.Perform(this, this);
			}

			Effects.RemoveAt(index);

			EffectRemoved?.Invoke(effect);
		}

		public void SpawnMissile(MissileDesc desc, Unit target)
		{
			TempNameManager.Spawn(desc, this, transform.position, target);
		}

		public void SpawnMissile(MissileDesc desc, Vector3 target)
		{
			TempNameManager.Spawn(desc, this, transform.position, target);
		}

		public void SpawnChain(ChainDesc desc, Unit target)
		{
			TempNameManager.Spawn(desc, transform.position, this, target, FactionID);
		}

		public void Extract(ResourceSource resourceSource, int amount)
		{
			ResourceExtractionHadler.Extract(resourceSource, amount, FactionID);
		}

		internal void Die()
		{
			IsDead = true;

			DeathCancellationTokenSource.Cancel();
			DeathCancellationTokenSource.Dispose();
			DeathCancellationTokenSource = null;

			HitBox.enabled = false;

			NavMeshAgent.enabled = false;

			Died?.Invoke();
		}
	}
}
