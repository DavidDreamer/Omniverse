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
			var spawner = SystemAPI.ManagedAPI.GetSingleton<UnitSpawner>();
			var prefab = spawner.Unit;

			var worldName = state.WorldUnmanaged.Name;

			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
			networkIdFromEntity.Update(ref state);

			foreach (var (receiver, requestEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestCommand>().WithEntityAccess())
			{
				Entity networkEntity = receiver.ValueRO.SourceConnection;
				NetworkId networkId = networkIdFromEntity[networkEntity];
				int factionId = networkId.Value - 1;

				commandBuffer.AddComponent(networkEntity, new Player
				{
					FactionID = factionId
				});

				commandBuffer.AddComponent<NetworkStreamInGame>(networkEntity);

				UnityEngine.Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game");

				var unit = commandBuffer.Instantiate(prefab);
				var localTransform = LocalTransform.FromPosition(spawner.SpawnPoints[factionId]);
				commandBuffer.SetComponent(unit, localTransform);
				commandBuffer.SetComponent(unit, new Faction
				{
					ID = factionId
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

				CreateAbility(spawner.Ability);
				CreateAbility(spawner.Ability2);

				void CreateAbility(Entity prefab)
				{
					Entity ability = commandBuffer.Instantiate(prefab);
					commandBuffer.SetComponent(ability, ghostOwner);
					commandBuffer.AppendToBuffer(receiver.ValueRO.SourceConnection, new LinkedEntityGroup
					{
						Value = ability
					});
					commandBuffer.AppendToBuffer(unit, new AbilityReference
					{
						Entity = ability
					});
					commandBuffer.SetComponent(ability, new Owner
					{
						Entity = unit
					});
				}

				commandBuffer.DestroyEntity(requestEntity);
			}

			commandBuffer.Playback(state.EntityManager);
		}
	}
}
