using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse.Units
{
	public class Unit : FactiousEntity<UnitDesc>, IPoolObject
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

		public Queue<ICommand> CommandsQueue { get; } = new();

		public ICommand Command { get; private set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		[Inject]
		private OperationHandler OperationHandler { get; set; }

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
				var ability = new Ability(abilityDesc, this, OperationHandler);
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

			NavMeshAgent.enabled = true;
		}

		public void FixedTick(float deltaTime)
		{
			if (IsDead)
			{
				return;
			}

			UpdateEffects(deltaTime);
			UpdateProperties();
			ProcessCommands(deltaTime);

			//NavMeshAgent.isStopped = Status.HasFlag(UnitStatus.Stunned);

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

		private void ProcessCommands(float deltaTime)
		{
			if (Command == null)
			{
				if (CommandsQueue.Count == 0)
				{
					return;
				}
				else
				{
					Command = CommandsQueue.Dequeue();
					Command.Start();
				}
			}

			while (true)
			{
				bool completed = Command.Tick(deltaTime);
				if (completed)
				{
					Command.Cleanup();

					if (CommandsQueue.Count > 0)
					{
						Command = CommandsQueue.Dequeue();
						Command.Start();
					}
					else
					{
						Command = null;
						break;
					}
				}
				else
				{
					break;
				}
			}
		}

		public void AddCommand(ICommand command, bool forced)
		{
			if (forced)
			{
				ClearCommands();
			}

			CommandsQueue.Enqueue(command);
		}

		public void ClearCommands()
		{
			if (Command != null)
			{
				Command.Cleanup();
				Command = null;
			}

			CommandsQueue.Clear();
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

			if (effect.Desc.OnRemovedOperation != null)
			{
				var context = new OperationContext(this);
				context.Points.Add(transform.position);
				OperationHandler.PerformAsync(effect.Desc.OnRemovedOperation, this, context, default).Forget();
			}

			Effects.RemoveAt(index);

			EffectRemoved?.Invoke(effect);
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
