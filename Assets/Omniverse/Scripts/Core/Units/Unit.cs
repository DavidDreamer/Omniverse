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
		
		private AttackModule AttackModule { get; set; }
		
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

			AttackModule = new AttackModule(desc.Attack);
		}

		public void FixedTick()
		{
			foreach (var resource in Properties)
			{
				resource.Value.FixedTick();
			}

			UpdateEffects(Time.fixedDeltaTime);
		}

		public void Tick()
		{
			if (IsDead)
			{
				return;
			}

			if (Locked)
			{
				return;
			}
			
			Presenter.Tick();
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

		public void AddForce(Vector3 force) => Presenter.AddForce(force);

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
		}
	}
}
