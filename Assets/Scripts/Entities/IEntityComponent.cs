using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IEntityComponent<in TEntity>
	{
		void Initialize(TEntity entity);
	}

	public interface IRendererComponent
	{
		List<Renderer> Renderers { get; }
	}

	public abstract class EntityComponent<TEntity> : MonoBehaviour, IEntityComponent<TEntity>
	{
		public TEntity Entity { get; private set; }

		public virtual void Initialize(TEntity entity)
		{
			Entity = entity;
		}
	}

	public abstract class RendererComponent<TEntity> : EntityComponent<TEntity>, IRendererComponent
	{
		[field: SerializeField]
		public List<Renderer> Renderers { get; private set; }

		[field: SerializeField]
		public Transform Center { get; private set; }
	}
}
