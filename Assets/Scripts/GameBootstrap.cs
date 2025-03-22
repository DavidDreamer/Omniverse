using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	public struct GoInGameRequest : IRpcCommand
	{
	}

	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
	public partial struct GoInGameClientSystem : ISystem
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
			foreach (var (id, entity) in SystemAPI.Query<RefRO<NetworkId>>().WithEntityAccess().WithNone<NetworkStreamInGame>())
			{
				commandBuffer.AddComponent<NetworkStreamInGame>(entity);
				Entity requestEntity = commandBuffer.CreateEntity();
				commandBuffer.AddComponent<GoInGameRequest>(requestEntity);
				var request = new SendRpcCommandRequest
				{
					TargetConnection = entity
				};
				commandBuffer.AddComponent(requestEntity, request);
			}
			commandBuffer.Playback(state.EntityManager);
		}
	}

	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct GoInGameServerSystem : ISystem
	{
		private ComponentLookup<NetworkId> networkIdFromEntity;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<UnitSpawner>();

			var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<GoInGameRequest>().WithAll<ReceiveRpcCommandRequest>();
			var query = state.GetEntityQuery(builder);
			state.RequireForUpdate(query);
			networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var prefab = SystemAPI.GetSingleton<UnitSpawner>().Unit;

			var worldName = state.WorldUnmanaged.Name;

			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
			networkIdFromEntity.Update(ref state);

			foreach (var (receiver, requestEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequest>().WithEntityAccess())
			{
				commandBuffer.AddComponent<NetworkStreamInGame>(receiver.ValueRO.SourceConnection);

				var networkId = networkIdFromEntity[receiver.ValueRO.SourceConnection];

				UnityEngine.Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game");

				var unit = commandBuffer.Instantiate(prefab);
				var localTransform = new LocalTransform
				{
					Position = new float3(16, 0, 16),
					Rotation = quaternion.identity,
					Scale = 1
				};
				commandBuffer.SetComponent(unit, localTransform);
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

	[UnityEngine.Scripting.Preserve]
	public class GameBootstrap : ClientServerBootstrap
	{
		public override bool Initialize(string defaultWorldName)
		{
			AutoConnectPort = 7979;
			return base.Initialize(defaultWorldName);
		}
	}
}
