using UnityEngine;
using VContainer;

namespace Omniverse
{
	public abstract class Entity : MonoBehaviour, IFactious
	{
		[field: SerializeField]
		public Collider HitBox { get; private set; }

		public int FactionID { get; private set; } = -1;

		[Inject]
		protected IObjectResolver ObjectResolver { get; set; }

		[Inject]
		public ResourceExtractionHadler ResourceExtractionHadler { get; private set; }

		[Inject]
		protected TempNameManager TempNameManager { get; set; }

		[Inject]
		public PhysicsService PhysicsService { get; private set; }

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
