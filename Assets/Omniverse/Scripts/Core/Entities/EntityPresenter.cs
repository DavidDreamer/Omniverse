using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IEntityPresenter
	{
		IEntity Entity { get; }
		
		GameObject GameObject { get; }
		
		List<Renderer> Renderers { get; }
	}
	
	public abstract class EntityPresenter<TEntity, TDesc>: MonoBehaviour, IEntityPresenter
		where TEntity: Entity<TDesc>
		where TDesc: EntityDesc
	{
		[field: SerializeField]
		public Collider HitBox { get; private set; }
		
		[field: SerializeField]
		public List<Renderer> Renderers { get; private set; }
		
		public TEntity Entity { get; private set; }

		public GameObject GameObject => gameObject;

		IEntity IEntityPresenter.Entity => Entity;

		public void Bind(TEntity entity)
		{
			Entity = entity;
		}
	}
}
