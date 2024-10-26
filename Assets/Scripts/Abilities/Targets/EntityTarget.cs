using UnityEngine;

namespace Omniverse.Abilities
{
	public abstract class EntityTarget<TEntity> : ITarget where TEntity : Entity
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }
	}
}
