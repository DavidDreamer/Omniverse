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
				movement.ValueRW.Speed.Reset();
				movement.ValueRW.TurnRate.Reset();
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
				movement.ValueRW.Speed.CalculateTotal();
				movement.ValueRW.TurnRate.CalculateTotal();
			}
		}
	}
}
