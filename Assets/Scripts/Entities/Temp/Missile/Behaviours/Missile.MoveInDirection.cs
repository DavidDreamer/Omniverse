using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	public partial class Missile
	{
		private class MoveInDirection : Behaviour
		{
			private Vector3 Direction { get; }

			private float Distance { get; set; }

			public MoveInDirection(Missile missile, Vector3 direciton) : base(missile)
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

				//TODO
				bool hit = false;

				foreach (var item in Missile.PhysicsService.GetEntitiesInSphere<Unit>(Missile.transform.position, radius).Where(unit => Missile.Desc.Filter.Match(Missile, unit)))
				{
					hit = true;
					Missile.PerformHitAction(item);
				}

				if (hit)
				{
					Missile.Completed = true;
				}
			}
		}
	}
}
