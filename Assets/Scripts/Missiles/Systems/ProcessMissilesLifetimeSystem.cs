using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct ProcessMissilesLifetimeSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

			foreach ((var missile, var transform, var range, var entity) in
				SystemAPI.Query<RefRW<Missile>, RefRO<LocalTransform>, RefRW<Range>>().WithEntityAccess().WithAll<Simulate>())
			{
				float distanceSq = math.distancesq(transform.ValueRO.Position, missile.ValueRO.StartPosition);
				float rangeSq = range.ValueRO.Value * range.ValueRO.Value;

				if (distanceSq >= rangeSq)
				{
					commandBuffer.DestroyEntity(entity);
				}
			}

			commandBuffer.Playback(state.EntityManager);
		}
	}
}
