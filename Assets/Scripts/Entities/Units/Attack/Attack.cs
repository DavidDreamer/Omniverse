using System;
using UnityEngine;

namespace Omniverse
{
	public class Attack
	{
		public event Action Started;

		public event Action<Entity> Performed;

		private Entity Entity { get; }

		public bool InProcess { get; private set; }

		public Attack(Entity entity)
		{
			Entity = entity;
		}

		public bool TargetIsInRange(Unit target)
		{
			float sqrDistance = Vector3.SqrMagnitude(Entity.transform.position - target.transform.position);
			float attackRange = Entity.Properties[PropertyID.AttackRange].Amount;
			float sqrAttackRange = attackRange * attackRange;
			return sqrDistance <= sqrAttackRange;
		}

		private float attackTime;

		public void BeginAttack()
		{
			attackTime = 0;
			Started?.Invoke();
			InProcess = true;
		}

		public void Tick(Unit target, float deltaTime)
		{
			float attackSpeed = Entity.Properties[PropertyID.AttackSpeed].Amount;

			float time = 1f / attackSpeed;
			TimeSpan timeSpan = TimeSpan.FromSeconds(time);

			attackTime += deltaTime;

			if (attackTime >= time)
			{
				if (TargetIsInRange(target))
				{
					var modifier = new PropertyModifier
					{
						Value = -Entity.Properties[PropertyID.AttackDamage].Amount,
					};

					target.ModifyProperty(PropertyID.Health, modifier, Entity);
				}

				Performed?.Invoke(target);

				InProcess = false;
			}
		}
	}
}
