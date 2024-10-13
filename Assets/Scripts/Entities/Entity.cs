using UnityEngine;

namespace Omniverse
{
	public abstract class Entity : MonoBehaviour
	{
		[field: SerializeField]
		public Collider HitBox { get; private set; }
	}

	public abstract class Entity<TDesc> : Entity where TDesc : EntityDesc
	{
		public TDesc Desc { get; set; }

		public virtual void Initialize(TDesc desc)
		{
			Desc = desc;
		}
	}
}
