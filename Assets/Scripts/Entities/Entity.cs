using UnityEngine;

namespace Omniverse
{
	public abstract class Entity : MonoBehaviour, IFactious
	{
		[field: SerializeField]
		public Collider HitBox { get; private set; }

		public int FactionID { get; private set; } = -1;

		public void ChangeFaction(int factionID)
		{
			FactionID = factionID;
		}
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
