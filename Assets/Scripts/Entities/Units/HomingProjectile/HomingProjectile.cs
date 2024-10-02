using Omniverse.Actions;
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
				var executionContext = new ExecutionContext();
				executionContext.Entities.Add(Target);
				Desc.HitAction.Perform(executionContext, default);
		
				Destroy(gameObject);
			}
		}
	}
}
