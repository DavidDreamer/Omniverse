using UnityEngine;

namespace Omniverse
{
	public partial class MissileObsolete
	{
		private class MoveToTarget : Behaviour
		{
			private DynamicEntity Target { get; }

			public MoveToTarget(MissileObsolete missile, DynamicEntity target) : base(missile)
			{
				Target = target;
			}

			public override void Tick(float deltaTime)
			{
				//TOD ECS
				//float speed = Missile.Desc.Speed;
				//float radius = Missile.Desc.Radius;

				//Vector3 targetPosition = Target.HitBox.transform.position;
				//Vector3 direction = (targetPosition - Missile.transform.position).normalized;
				//Missile.transform.position += direction * speed * deltaTime;

				//float sqrDistanceToTarget = Vector3.SqrMagnitude(targetPosition - Missile.transform.position);

				//if (sqrDistanceToTarget <= radius * radius)
				//{
				//	Missile.PerformHitAction((UnitObsolete)Target);
				//	Missile.Completed = true;
				//}
			}
		}
	}
}
