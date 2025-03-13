using System.Collections.Generic;
using Omniverse.Abilities;
using UnityEngine.AI;

namespace Omniverse
{
	public class UnitObsolete : OmniverseEntity<UnitDesc>, IPoolObject
	{
		public event System.Action Died;

		public List<AbilityObsolete> Abilities { get; } = new();

		public bool IsDead { get; private set; }

		public UnitStatus Status { get; private set; }

		public Experience Experience { get; private set; }

		public Inventory Inventory { get; private set; }

		public CommandModule CommandModule { get; private set; }

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
				var ability = new AbilityObsolete(abilityDesc, this);
				Abilities.Add(ability);
			}

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

			bool isStunned = Status.HasFlag(UnitStatus.Stunned);
			NavMeshAgent.isStopped = isStunned;
			if (isStunned)
			{
			}
			else
			{
				CommandModule.Tick(deltaTime);
			}

			void UpdateAbilities()
			{
				foreach (AbilityObsolete ability in Abilities)
				{
					ability.Tick(deltaTime);
				}
			}

			void UpdateEffects()
			{
				Status = 0;

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

		internal void Die()
		{
			IsDead = true;

			HitBox.enabled = false;

			NavMeshAgent.enabled = false;

			Died?.Invoke();
		}
	}
}
