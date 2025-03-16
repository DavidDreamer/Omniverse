using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse
{
	public abstract partial class OmniverseEntity : MonoBehaviour, IFactious
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

		public AttackModule Attack { get; protected set; }

		[Inject]
		protected IObjectResolver ObjectResolver { get; set; }

		[Inject]
		protected TempNameManager TempNameManager { get; set; }

		public void ChangeFaction(int factionID)
		{
			FactionID = factionID;
		}

		public void ModifyProperty(PropertyID propertyID, PropertyModifier modifier, OmniverseEntity source)
		{
			Property property = Properties[propertyID];
			property.Modify(modifier);
		}

		public void ApplyEffect(EffectDesc effectDesc)
		{
			Effect effect = new(this, effectDesc);

			Effects.Add(effect);

			effect.Desc.OnAppliedOperation?.Perform(this, None.Instance);

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

			effect.Desc.OnRemovedOperation?.Perform(this, None.Instance);

			Effects.RemoveAt(index);

			EffectRemoved?.Invoke(effect);
		}

		public void SpawnMissile(MissileDesc desc, OmniverseEntity target)
		{
			TempNameManager.Spawn(desc, this, HitBox.transform.position, target);
		}

		public void SpawnMissile(MissileDesc desc, Vector3 target)
		{
			TempNameManager.Spawn(desc, this, HitBox.transform.position, target);
		}

		public void SpawnChain(ChainDesc desc, OmniverseEntity target)
		{
			TempNameManager.Spawn(desc, HitBox.transform.position, this, target, FactionID);
		}

		public void Extract(ResourceSource resourceSource, int amount)
		{
			//TODO ECS
			//var factionsData = ECSUtils.GetSingleton<FactionsData>();
			//resourceSource.Extract(ref amount);
			//FactionObsolete faction = factionsData[factionID];
			//faction.ChangeResource(resourceSource.Desc.Resource, amount);
		}
	}

	public abstract class OmniverseEntity<TDesc> : OmniverseEntity where TDesc : EntityDesc
	{
		public TDesc Desc { get; set; }

		public virtual void Initialize(TDesc desc)
		{
			Desc = desc;

			Attack = new AttackModule(this);
		}
	}
}
