using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Units
{
	public class Attack
	{
		public event Action Started;

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

		public bool CanAttack(Unit target) => !InProcess && target.IsEnemyFor(Unit) && TargetIsInRange(target);

		public async UniTaskVoid Perform(Unit target, CancellationToken token)
		{
			InProcess = true;

			float attackSpeed = Unit.Properties[PropertyID.AttackSpeed].Amount.Value;

			float time = 1f / attackSpeed;
			TimeSpan timeSpan = TimeSpan.FromSeconds(time);

			Started?.Invoke();

			await UniTask.Delay(timeSpan, cancellationToken: token);

			if (TargetIsInRange(target))
			{
				var data = new ChangePropertyData
				{
					Amount = -Unit.Properties[PropertyID.AttackDamage].Amount,
					ID = target.Properties.Keys.First()
				};

				target.ModifyProperty(data, Unit);
			}

			InProcess = false;
		}
	}
}
