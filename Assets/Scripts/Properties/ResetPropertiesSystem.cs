using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	[DisableAutoCreation]
	public partial struct ResetPropertiesSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var movement in SystemAPI.Query<RefRW<Movement>>().WithAll<Simulate>())
			{
				movement.ValueRW.Speed.Additional = 0;
				movement.ValueRW.Speed.Multipler = 1;
			}
		}
	}

	[BurstCompile]
	[DisableAutoCreation]
	public partial struct CalculatePropertiesTotalSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var movement in SystemAPI.Query<RefRW<Movement>>().WithAll<Simulate>())
			{
				movement.ValueRW.Speed.Total = (movement.ValueRW.Speed.Base + movement.ValueRW.Speed.Additional) * movement.ValueRW.Speed.Multipler;
			}
		}
	}
}
