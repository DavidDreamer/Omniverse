using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[DisableAutoCreation]
	public partial struct ApplyAreaModifiersSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var map = SystemAPI.GetSingleton<Map>();

			foreach ((var localTransform, var movement) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Movement>>().WithAll<Simulate>())
			{
				Node node = map.NodeFromPosition(localTransform.ValueRO.Position);
				float penalty = map.Penalties[node.Id];

				Property speed = movement.ValueRW.Speed;
				speed.Multipler += 1 / penalty - 1;
				movement.ValueRW.Speed = speed;
			}
		}
	}
}
