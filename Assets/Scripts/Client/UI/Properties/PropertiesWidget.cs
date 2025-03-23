using UnityEngine;

namespace Omniverse.UI
{
	public class PropertiesWidget : MonoBehaviour
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

		private UnitObsolete Unit { get; set; }

		public void Bind(UnitObsolete unit)
		{
			Unit = unit;

			AttackDamage.Bind(Unit.Properties[PropertyID.AttackDamage]);
			AttackSpeed.Bind(Unit.Properties[PropertyID.AttackSpeed]);
			AttackRange.Bind(Unit.Properties[PropertyID.AttackRange]);
			MovementSpeed.Bind(Unit.Properties[PropertyID.MovementSpeed]);
			RotationSpeed.Bind(Unit.Properties[PropertyID.RotationSpeed]);
			VisionRange.Bind(Unit.Properties[PropertyID.VisionRange]);
		}
	}
}
