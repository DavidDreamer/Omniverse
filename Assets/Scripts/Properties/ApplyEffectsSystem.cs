using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	[DisableAutoCreation]
	public partial struct ApplyEffectsSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach ((var unit, var effectBuffer, var entity) in SystemAPI.Query<RefRW<Unit>, DynamicBuffer<Effect>>().WithEntityAccess().WithAll<Simulate>())
			{
				for (int i = 0; i < effectBuffer.Length; i++)
				{
					Effect effect = effectBuffer.ElementAt(i);
					var propertyModifiers = effect.Desc.Value.PropertyModifiers;

					foreach (var propertyModifier in propertyModifiers)
					{
						Property property = GetProperty(ref state, entity, propertyModifier.ID);

						switch (propertyModifier.Modifier.Mode)
						{
							case PropertyModifierMode.Addition:
								property.Additional += propertyModifier.Modifier.Value;
								break;
							case PropertyModifierMode.Multiplication:
								property.Multipler += propertyModifier.Modifier.Value;
								break;

						}
	
						SetProperty(ref state, entity, propertyModifier.ID, property);
					}
				}
			}
		}

		Property GetProperty(ref SystemState state, Entity entity, PropertyID propertyID)
		{
			switch (propertyID)
			{
				case PropertyID.MovementSpeed:
					return SystemAPI.GetComponent<Movement>(entity).Speed;
				default: throw new System.Exception();

			}
		}

		void SetProperty(ref SystemState state, Entity entity, PropertyID propertyID, Property property)
		{
			switch (propertyID)
			{
				case PropertyID.MovementSpeed:
					SystemAPI.GetComponentRW<Movement>(entity).ValueRW.Speed = property;
					break;

			}
		}
	}
}
