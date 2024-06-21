using UnityEngine;

namespace Omniverse
{
	public abstract class EntityPresenter: MonoBehaviour
	{
		[field: SerializeField]
		public Collider HitBox { get; private set; }
		
		public abstract int FactionID { get; }
	}
	
	public abstract class EntityPresenter<TEntity, TDesc>: EntityPresenter
		where TEntity: Entity<TDesc>
		where TDesc: EntityDesc
	{
		public TEntity Entity { get; set; }

		public override int FactionID => Entity.FactionID;
	}
}
