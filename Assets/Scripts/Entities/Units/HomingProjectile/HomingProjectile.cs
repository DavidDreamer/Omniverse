using UnityEngine;
using VContainer;

namespace Omniverse.Units
{
	public class HomingProjectile : Entity<HomingProjectileDesc>, IFactious
	{
		[Inject]
		private ActionHandler ActionHandler { get; set; }

		public Unit Target { get; set; }

		public Unit Owner { get; set; }

		public int FactionID { get; set; }

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
				var context = new ActionContext(Owner);
				context.Entities.Add(Target);
				ActionHandler.Perform(Desc.HitAction, this, context);
				Destroy(gameObject);
			}
		}
	}
}
