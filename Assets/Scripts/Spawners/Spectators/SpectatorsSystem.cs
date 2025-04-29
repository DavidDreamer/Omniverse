using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[WorldSystemFilter(WorldSystemFilterFlags.Presentation)]
	public partial struct SpectatorsSystem : ISystem, ISystemStartStop
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<Spectators>();
		}

		public void OnStartRunning(ref SystemState state)
		{
			EntityManager entityManager = state.EntityManager;

			var spectators = SystemAPI.GetSingleton<Spectators>();
			var spectatorsEntity = SystemAPI.GetSingletonEntity<Spectators>();
			var spawnPoints = state.EntityManager.GetBuffer<SpawnPoint>(spectatorsEntity);

			foreach (SpawnPoint point in spawnPoints)
			{
				Entity spectator = entityManager.Instantiate(spectators.Prefab);
				var localTransform = LocalTransform.FromPositionRotation(point.Position, point.Rotation);
				entityManager.SetComponentData(spectator, localTransform);
			}
		}

		public void OnStopRunning(ref SystemState state)
		{
		}
	}
}
