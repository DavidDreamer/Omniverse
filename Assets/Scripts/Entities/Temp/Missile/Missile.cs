using UnityEngine;
using VContainer;

namespace Omniverse
{
	public partial class Missile : TempName
	{
		private MissileDesc Desc { get; set; }

		private Entity Owner { get; set; }

		private Behaviour behaviour { get; set; }

		[Inject]
		private PhysicsService PhysicsService { get; set; }

		private void Initialize(MissileDesc desc, Unit owner)
		{
			Desc = desc;
			Owner = owner;
			ChangeFaction(owner.FactionID);
		}

		public void Initialize(MissileDesc desc, Unit owner, Vector3 vector)
		{
			Initialize(desc, owner);
			behaviour = new MoveInDirection(this, vector);
		}

		public void Initialize(MissileDesc desc, Unit owner, Unit target)
		{
			Initialize(desc, owner);
			behaviour = new MoveToTarget(this, target);
		}

		public override void Tick(float deltaTime)
		{
			behaviour.Tick(deltaTime);
		}

		private void PerformHitAction(Unit target)
		{
			Desc.HitAction.Perform(Owner as Unit, target);
		}
	}
}
