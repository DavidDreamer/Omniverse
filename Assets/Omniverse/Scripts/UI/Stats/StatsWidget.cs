using Omniverse.Units;
using UnityEngine;

namespace Omniverse.UI
{
	public class StatsWidget: MonoBehaviour
	{
		[field: SerializeField]
		private StatWidget AttackDamage { get; set; }
		
		[field: SerializeField]
		private StatWidget AttackSpeed { get; set; }
		
		[field: SerializeField]
		private StatWidget AttackRange { get; set; }
		
		[field: SerializeField]
		private StatWidget MovementSpeed { get; set; }
		
		[field: SerializeField]
		private StatWidget RotationSpeed { get; set; }
		
		private Unit Unit { get; set; }
		
		public void Bind(Unit unit)
		{
			Unit = unit;
		}

		public void LateUpdate()
		{
			if (Unit is null)
			{
				return;
			}
			
			AttackDamage.Value.text = Unit.Attack.Damage.ToString();
			AttackSpeed.Value.text = Unit.Attack.Speed.ToString();
			AttackRange.Value.text = Unit.Attack.Range.ToString();
			MovementSpeed.Value.text = Unit.Movement.Speed.ToString();
			RotationSpeed.Value.text = Unit.Movement.RotationSpeed.ToString();
		}
	}
}
