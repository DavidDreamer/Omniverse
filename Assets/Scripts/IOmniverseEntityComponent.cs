using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IOmniverseEntityComponent<in TEntity>
	{
		void Initialize(TEntity entity);
	}

	public interface IRendererComponent
	{
		List<Renderer> Renderers { get; }
	}

	public abstract class OmniverseEntityComponent<TEntity> : MonoBehaviour, IOmniverseEntityComponent<TEntity>
	{
		public TEntity Entity { get; private set; }

		public virtual void Initialize(TEntity entity)
		{
			Entity = entity;
		}
	}

	public abstract class RendererComponent<TEntity> : OmniverseEntityComponent<TEntity>, IRendererComponent
	{
		[field: SerializeField]
		public List<Renderer> Renderers { get; private set; }
	}
}
