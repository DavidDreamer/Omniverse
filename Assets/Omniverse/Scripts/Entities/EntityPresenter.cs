using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public abstract class EntityPresenter: MonoBehaviour
	{
		[field: SerializeField]
		public List<Renderer> Renderers { get; private set; }

		[field: SerializeField]
		public Collider HitBox { get; private set; }
	}

	public abstract class EntityPresenter<TEntity, TDesc>: EntityPresenter
		where TEntity: Entity<TDesc>
		where TDesc: EntityDesc
	{
		public TEntity Entity { get; private set; }

		public void Bind(TEntity entity)
		{
			Entity = entity;
		}
	}
}
