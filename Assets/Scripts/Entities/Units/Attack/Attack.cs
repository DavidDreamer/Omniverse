using System;
using UnityEngine;

namespace Omniverse.Units
{
	public class Attack
	{
		public event System.Action Started;

		private Unit Unit { get; }

		public bool InProcess { get; private set; }

		public Attack(Unit unit)
		{
			Unit = unit;
		}

		public bool TargetIsInRange(Unit target)
		{
			float sqrDistance = Vector3.SqrMagnitude(Unit.transform.position - target.transform.position);
			float attackRange = Unit.Properties[PropertyID.AttackRange].Amount.Value;
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
			float attackSpeed = Unit.Properties[PropertyID.AttackSpeed].Amount.Value;

			float time = 1f / attackSpeed;
			TimeSpan timeSpan = TimeSpan.FromSeconds(time);

			attackTime += deltaTime;

			if (attackTime >= time)
			{
				if (TargetIsInRange(target))
				{
					var modifier = new PropertyModifier
					{
						Value = -Unit.Properties[PropertyID.AttackDamage].Amount,
					};

					target.ModifyProperty(PropertyID.Health, modifier, Unit);
				}

				InProcess = false;
			}
		}
	}
}
