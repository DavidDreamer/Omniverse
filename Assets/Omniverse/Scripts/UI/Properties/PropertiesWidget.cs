using Omniverse.Units;
using UnityEngine;

namespace Omniverse.UI
{
	public class PropertiesWidget: MonoBehaviour
	{
		[field: SerializeField]
		private PropertyWidget AttackDamage { get; set; }
		
		[field: SerializeField]
		private PropertyWidget AttackSpeed { get; set; }
		
		[field: SerializeField]
		private PropertyWidget AttackRange { get; set; }
		
		[field: SerializeField]
		private PropertyWidget MovementSpeed { get; set; }
		
		[field: SerializeField]
		private PropertyWidget RotationSpeed { get; set; }
		
		[field: SerializeField]
		private PropertyWidget VisionRange { get; set; }
		
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
			MovementSpeed.Value.text = Unit.Properties[PropertyID.MovementSpeed].Amount.ToString();
			RotationSpeed.Value.text = Unit.Properties[PropertyID.RotationSpeed].Amount.ToString();
			VisionRange.Value.text = Unit.Desc.VisionRange.ToString();
		}
	}
}
