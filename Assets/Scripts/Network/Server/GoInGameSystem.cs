using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse.Network.Server
{
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial class GoInGameSystem : SystemBase
	{
		private ComponentLookup<NetworkId> networkIdFromEntity;

		protected override void OnCreate()
		{
			RequireForUpdate<UnitSpawner>();

			var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<GoInGameRequestCommand>().WithAll<ReceiveRpcCommandRequest>();
			var query = GetEntityQuery(builder);
			RequireForUpdate(query);
			networkIdFromEntity = GetComponentLookup<NetworkId>(true);
		}

		protected override void OnUpdate()
		{
			var spawner = SystemAPI.ManagedAPI.GetSingleton<UnitSpawner>();
			var prefab = spawner.Unit;

			var worldName = World.Unmanaged.Name;

			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
			networkIdFromEntity.Update(this);

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

				var mapSize = SystemAPI.GetSingleton<MapSettings>().Size / 2;

				SpawnUnit();

				void SpawnUnit()
				{
					var unit = commandBuffer.Instantiate(prefab);
					var localTransform = LocalTransform.FromPosition(spawner.SpawnPoints[factionId]);
					commandBuffer.SetComponent(unit, localTransform);
					commandBuffer.SetComponent(unit, new Faction
					{
						ID = factionId,
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
				}

				commandBuffer.DestroyEntity(requestEntity);
			}

			commandBuffer.Playback(EntityManager);
		}
	}
}
