using UnityEngine;

namespace Omniverse
{
	public abstract class EntityPresenter<TEntity, TDesc>: MonoBehaviour 
		where TEntity: Entity<TDesc>
		where TDesc: EntityDesc
	{
		public TEntity Entity { get; set; }
	}
}
