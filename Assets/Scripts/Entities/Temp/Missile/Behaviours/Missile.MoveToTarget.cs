using UnityEngine;

namespace Omniverse
{
	public partial class Missile
	{
		private class MoveToTarget : Behaviour
		{
			private Unit Target { get; }

			public MoveToTarget(Missile missile, Unit target) : base(missile)
			{
				Target = target;
			}

			public override void Tick(float deltaTime)
			{
				float speed = Missile.Desc.Speed;
				float radius = Missile.Desc.Radius;

				Vector3 targetPosition = Target.HitBox.transform.position;
				Vector3 direction = (targetPosition - Missile.transform.position).normalized;
				Missile.transform.position += direction * speed * deltaTime;

				float sqrDistanceToTarget = Vector3.SqrMagnitude(targetPosition - Missile.transform.position);

				if (sqrDistanceToTarget <= radius * radius)
				{
					Missile.PerformHitAction(Target);
					Missile.Completed = true;
				}
			}
		}
	}
}
