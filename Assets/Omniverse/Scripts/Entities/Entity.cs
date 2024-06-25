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
	
	public abstract class Entity<TDesc>: IEntity<TDesc> where TDesc: EntityDesc
	{
		public TDesc Desc { get; }

		public int FactionID { get; }
		
		public Dictionary<PropertyID, Property> Properties { get; }

		protected Entity(TDesc desc, int factionID)
		{
			Desc = desc;
			FactionID = factionID;

			Properties = new Dictionary<PropertyID, Property>();
		}
	}
}
