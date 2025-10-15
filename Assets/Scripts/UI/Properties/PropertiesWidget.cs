using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	//TODO ECS
	public class PropertiesWidget : Widget
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

		public void Bind(Entity entity)
		{
			//AttackDamage.Bind(Unit.Properties[PropertyID.AttackDamage]);
			//AttackSpeed.Bind(Unit.Properties[PropertyID.AttackSpeed]);
			//AttackRange.Bind(Unit.Properties[PropertyID.AttackRange]);

			var movement = EntityManager.GetComponentData<Movement>(entity);
			MovementSpeed.Bind(movement.Speed);

			//RotationSpeed.Bind(Unit.Properties[PropertyID.RotationSpeed]);
			//VisionRange.Bind(Unit.Properties[PropertyID.VisionRange]);
		}
	}
}
