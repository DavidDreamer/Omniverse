using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse
{
	public abstract class Entity : MonoBehaviour, IFactious
	{
		public event Action<Effect> EffectApplied;

		public event Action<Effect> EffectRemoved;

		[field: SerializeField]
		public Collider HitBox { get; private set; }

		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		public int FactionID { get; private set; } = -1;

		public Dictionary<PropertyID, Property> Properties { get; } = new();

		public List<Effect> Effects { get; } = new();

		[Inject]
		protected IObjectResolver ObjectResolver { get; set; }

		[Inject]
		public ResourceExtractionHadler ResourceExtractionHadler { get; private set; }

		[Inject]
		protected TempNameManager TempNameManager { get; set; }

		[Inject]
		public PhysicsService PhysicsService { get; private set; }

		public void ChangeFaction(int factionID)
		{
			FactionID = factionID;
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
				effect.Desc.OnRemovedOperation.Perform(this, None.Instance);
			}

			Effects.RemoveAt(index);

			EffectRemoved?.Invoke(effect);
		}

		public void SpawnMissile(MissileDesc desc, Entity target)
		{
			TempNameManager.Spawn(desc, this, HitBox.transform.position, target);
		}

		public void SpawnMissile(MissileDesc desc, Vector3 target)
		{
			TempNameManager.Spawn(desc, this, HitBox.transform.position, target);
		}

		public void SpawnChain(ChainDesc desc, Entity target)
		{
			TempNameManager.Spawn(desc, HitBox.transform.position, this, target, FactionID);
		}

		public void Extract(ResourceSource resourceSource, int amount)
		{
			ResourceExtractionHadler.Extract(resourceSource, amount, FactionID);
		}
	}

	public abstract class Entity<TDesc> : Entity where TDesc : EntityDesc
	{
		public TDesc Desc { get; set; }

		public virtual void Initialize(TDesc desc)
		{
			Desc = desc;
		}
	}
}
