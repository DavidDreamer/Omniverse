using UnityEngine.AI;

namespace Omniverse
{
	public class UnitObsolete : OmniverseEntity<UnitDesc>, IPoolObject
	{
		public event System.Action Died;

		public bool IsDead { get; private set; }

		public UnitStatus Status { get; private set; }

		public Experience Experience { get; private set; }

		public Inventory Inventory { get; private set; }

		public override void Initialize(UnitDesc desc)
		{
			base.Initialize(desc);

			Experience = new Experience(desc.Experience);

			foreach (PropertyDesc resourceDesc in Desc.Properties)
			{
				Properties.Add(resourceDesc.ID, new Property(resourceDesc));
			}

			Inventory = new Inventory(desc.Inventory);
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

			UpdateEffects();
			UpdateProperties();

			bool isStunned = Status.HasFlag(UnitStatus.Stunned);
			NavMeshAgent.isStopped = isStunned;

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
