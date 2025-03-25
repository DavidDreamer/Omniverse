using UnityEngine;

namespace Omniverse
{
	public partial class MissileObsolete
	{
		private class MoveInDirection : Behaviour
		{
			private Vector3 Direction { get; }

			private float Distance { get; set; }

			public MoveInDirection(MissileObsolete missile, Vector3 direciton) : base(missile)
			{
				Direction = direciton;
			}

			public override void Tick(float deltaTime)
			{
				float speed = Missile.Desc.Speed;
				float range = Missile.Desc.Range;
				float radius = Missile.Desc.Radius;

				float positionDelta = speed * deltaTime;
				float remainingDistance = range - Distance;
				positionDelta = Mathf.Min(positionDelta, remainingDistance);
				Distance += positionDelta;
				Missile.transform.position += Direction * positionDelta;

				if (Distance == range)
				{
					Missile.Completed = true;
					return;
				}

				//TODO ECS
				//bool hit = false;

				//foreach (var item in PhysicsService.GetEntitiesInSphere<UnitObsolete>(Missile, radius, Missile.Desc.Filter))
				//{
				//	hit = true;
				//	Missile.PerformHitAction(item);
				//}

				//if (hit)
				//{
				//	Missile.Completed = true;
				//}
			}
		}
	}
}
