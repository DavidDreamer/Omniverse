using System.Collections.Generic;
using Omniverse.Entities;
using UnityEngine;

namespace Omniverse
{
	public interface IEntity
	{
		int FactionID { get; }

		Dictionary<PropertyID, Property> Properties { get; }
	}

	public interface IEntity<out TDesc>: IEntity
	{
		TDesc Desc { get; }
	}

	public abstract class Entity: MonoBehaviour, IEntity
	{
		[field: SerializeField]
		public List<Renderer> Renderers { get; private set; }

		[field: SerializeField]
		public Collider HitBox { get; private set; }

		public int FactionID { get; set; }

		public Dictionary<PropertyID, Property> Properties { get; } = new();
	}
	
	public abstract class Entity<TDesc>: Entity, IEntity<TDesc> where TDesc: EntityDesc
	{
		public TDesc Desc { get; set; }
		
		public virtual void Initialize(TDesc desc, int factionID)
		{
			Desc = desc;
			FactionID = factionID;
		}
	}
}
