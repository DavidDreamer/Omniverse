using UnityEngine;

namespace Omniverse
{
	public partial class MissileObsolete : TempName
	{
		private MissileDesc Desc { get; set; }

		private DynamicEntity Owner { get; set; }

		private Behaviour behaviour { get; set; }

		private void Initialize(MissileDesc desc, DynamicEntity owner)
		{
			Desc = desc;
			Owner = owner;
		}

		public void Initialize(MissileDesc desc, DynamicEntity owner, Vector3 vector)
		{
			Initialize(desc, owner);
			behaviour = new MoveInDirection(this, vector);
		}

		public void Initialize(MissileDesc desc, DynamicEntity owner, DynamicEntity target)
		{
			Initialize(desc, owner);
			behaviour = new MoveToTarget(this, target);
		}

		public override void Tick(float deltaTime)
		{
			behaviour.Tick(deltaTime);
		}

		//TODO ECS
		//private void PerformHitAction(UnitObsolete target)
		//{
		//	Desc.HitOperation.Perform(Owner, target);
		//}
	}
}
