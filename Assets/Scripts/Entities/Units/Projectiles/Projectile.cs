using System.Linq;
using UnityEngine;
using VContainer;

namespace Omniverse.Units
{
	public class Projectile : FactiousEntity<ProjectileDesc>
	{
		//TODO
		[Inject]
		private PhysicsService PhysicsService { get; set; }

		public Vector3 Direction { get; set; }

		private float Distance { get; set; }

		public void FixedUpdate()
		{
			float deltaTime = Time.fixedDeltaTime;

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
				Destroy(gameObject);

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
				Destroy(gameObject);
			}
		}
	}
}
