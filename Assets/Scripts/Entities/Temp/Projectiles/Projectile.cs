using System.Linq;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class Projectile : TempName, IFactious
	{
		//TODO
		[Inject]
		private PhysicsService PhysicsService { get; set; }

		public Vector3 Direction { get; set; }

		private float Distance { get; set; }

		public int FactionID { get; set; }

		private ProjectileDesc Desc { get; set; }

		public void Initialize(ProjectileDesc desc)
		{
			Desc = desc;
		}

		public override void Tick(float deltaTime)
		{
			float speed = Desc.Speed;
			float range = Desc.Range;
			float radius = Desc.Radius;

			float positionDelta = Desc.Speed * deltaTime;
			float remainingDistance = Desc.Range - Distance;
			positionDelta = Mathf.Min(positionDelta, remainingDistance);
			Distance += positionDelta;
			transform.position += Direction * positionDelta;

			if (Distance == range)
			{
				Completed = true;
				return;
			}

			var filter = FactiousFilter.Enemy;

			//TODO
			bool hit = false;

			foreach (var item in PhysicsService.GetEntitiesInSphere<Unit>(transform.position, radius).Where(unit => filter.Match(this, unit)))
			{
				hit = true;

				var modifier = new PropertyModifier()
				{
					Value = -10
				};

				item.ModifyProperty(PropertyID.Health, modifier, this);
			}

			if (hit)
			{
				Completed = true;
			}
		}
	}
}
