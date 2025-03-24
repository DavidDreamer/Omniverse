using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse.Network.Client
{
	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
	public partial struct GoInGameSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<UnitSpawner>();

			var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<NetworkId>().WithNone<NetworkStreamInGame>();
			var query = state.GetEntityQuery(builder);
			state.RequireForUpdate(query);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
			foreach (var (networkId, entity) in SystemAPI.Query<RefRO<NetworkId>>().WithEntityAccess().WithNone<NetworkStreamInGame>())
			{
				var player = new Player
				{
					FactionID = networkId.ValueRO.Value - 1
				};

				commandBuffer.AddComponent(entity, player);
				commandBuffer.AddComponent<NetworkStreamInGame>(entity);
				Entity requestEntity = commandBuffer.CreateEntity();
				commandBuffer.AddComponent<GoInGameRequestCommand>(requestEntity);
				var request = new SendRpcCommandRequest
				{
					TargetConnection = entity
				};
				commandBuffer.AddComponent(requestEntity, request);
			}
			commandBuffer.Playback(state.EntityManager);
		}
	}
}
