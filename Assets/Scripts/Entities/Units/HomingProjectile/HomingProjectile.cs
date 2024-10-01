using UnityEngine;

namespace Omniverse.Units
{
	public class HomingProjectile : FactiousEntity<HomingProjectileDesc>
	{
		public Unit Target { get; set; }

		public void FixedUpdate()
		{
			float deltaTime = Time.fixedDeltaTime;

			float speed = Desc.Speed;
			float radius = Desc.Radius;

			Vector3 direction = (Target.transform.position - transform.position).normalized;
			transform.position += direction * speed * deltaTime;

			float sqrDistanceToTarget = Vector3.SqrMagnitude(Target.transform.position - transform.position);

			if (sqrDistanceToTarget <= radius * radius)
			{
				var data = new ChangePropertyData()
				{
					ID = PropertyID.Health,
					Amount = -10
				};

				Target.ChangeResource(data);
				Destroy(gameObject);
			}
		}
	}
}
