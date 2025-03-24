using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse.Network.Server
{
	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct GoInGameSystem : ISystem
	{
		private ComponentLookup<NetworkId> networkIdFromEntity;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<UnitSpawner>();

			var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<GoInGameRequestCommand>().WithAll<ReceiveRpcCommandRequest>();
			var query = state.GetEntityQuery(builder);
			state.RequireForUpdate(query);
			networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var spawnerEntity = SystemAPI.GetSingletonEntity<UnitSpawner>();
			var spawnerLocalTransform = SystemAPI.GetComponent<LocalTransform>(spawnerEntity);
			var spawner = SystemAPI.GetSingleton<UnitSpawner>();
			var prefab = spawner.Unit;

			var worldName = state.WorldUnmanaged.Name;

			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
			networkIdFromEntity.Update(ref state);

			foreach (var (receiver, requestEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestCommand>().WithEntityAccess())
			{
				commandBuffer.AddComponent<NetworkStreamInGame>(receiver.ValueRO.SourceConnection);

				var networkId = networkIdFromEntity[receiver.ValueRO.SourceConnection];

				UnityEngine.Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game");

				var unit = commandBuffer.Instantiate(prefab);
				commandBuffer.SetComponent(unit, spawnerLocalTransform);
				commandBuffer.SetComponent(unit, new Faction
				{
					ID = networkId.Value - 1
				});
				var ghostOwner = new GhostOwner
				{
					NetworkId = networkId.Value
				};
				commandBuffer.SetComponent(unit, ghostOwner);
				var linkedEntityGroup = new LinkedEntityGroup
				{
					Value = unit
				};
				commandBuffer.AppendToBuffer(receiver.ValueRO.SourceConnection, linkedEntityGroup);

				commandBuffer.DestroyEntity(requestEntity);
			}

			commandBuffer.Playback(state.EntityManager);
		}
	}
}
