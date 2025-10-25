using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial class MapSystemGroup : ComponentSystemGroup
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			EnableSystemSorting = false;

			CreateSystem<UpdateObstaclesSystem>();

			void CreateSystem<T>() where T : unmanaged, ISystem
			{
				SystemHandle systemHandle = World.CreateSystem<T>();
				AddSystemToUpdateList(systemHandle);
			}
		}

		protected override void OnStartRunning()
		{
			base.OnStartRunning();

			var settings = SystemAPI.GetSingleton<MapSettings>();

			var entity = EntityManager.CreateEntity();
			EntityManager.SetName(entity, "Map");

			EntityManager.AddComponent<Map>(entity);

			int nodesCount = settings.Size.x * settings.Size.y;

			var nodes = new NativeArray<Node>(nodesCount, Allocator.Persistent);

			for (int i = 0; i < settings.Size.x; i++)
			{
				for (int j = 0; j < settings.Size.y; j++)
				{
					int id = j * settings.Size.x + i;
					nodes[id] = new()
					{
						Id = id,
						Coordinates = new(i - settings.Size.x / 2, j - settings.Size.y / 2),
					};
				}
			}

			Map map = new()
			{
				Size = settings.Size,
				Nodes = nodes
			};

			EntityManager.SetComponentData(entity, map);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (SystemAPI.HasSingleton<Map>())
			{
				var map = SystemAPI.GetSingleton<Map>();
				map.Nodes.Dispose();
			}
		}
	}

	[BurstCompile]
	[DisableAutoCreation]
	public partial struct UpdateObstaclesSystem : ISystem
	{
		private EntityQuery _obstaclesQuery;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			_obstaclesQuery = SystemAPI.QueryBuilder().WithAll<Obstacle>().Build();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var netrowkTime = SystemAPI.GetSingleton<NetworkTime>();

			if (!netrowkTime.IsFirstTimeFullyPredictingTick)
			{
				return;
			}

			var mapSettings = SystemAPI.GetSingleton<MapSettings>();
			var map = SystemAPI.GetSingletonRW<Map>();

			var obstacles = _obstaclesQuery.ToComponentDataArray<Obstacle>(Allocator.Temp);

			var nodes = map.ValueRW.Nodes.AsReadOnlySpan();

			for (int i = 0; i < nodes.Length; i++)
			{
				var node = nodes[i];

				bool obs = false;

				foreach (Obstacle obstacle in obstacles.AsReadOnlySpan())
				{
					obs |= node.Coordinates.x >= obstacle.Start.x
						&& node.Coordinates.y >= obstacle.Start.y
						&& node.Coordinates.x <= obstacle.End.x
						&& node.Coordinates.y <= obstacle.End.y;
				}

				node.Obstacle = obs;

				map.ValueRW.Nodes[i] = node;
			}

			obstacles.Dispose();
		}
	}
}
