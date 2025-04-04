using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct MoveMissilesSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach ((var missile, var trasnform) in SystemAPI.Query<RefRW<Missile>, RefRW<LocalTransform>>().WithAll<Simulate>())
			{
				trasnform.ValueRW = trasnform.ValueRW.Translate(missile.ValueRW.Direction * missile.ValueRW.Speed * SystemAPI.Time.fixedDeltaTime);
			}
		}
	}
}
