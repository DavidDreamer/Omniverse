using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

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

			int halfX = settings.Size.x / 2;
			int halfY = settings.Size.y / 2;

			for (int i = 0; i < settings.Size.x; i++)
			{
				for (int j = 0; j < settings.Size.y; j++)
				{
					var tempNodes = new NativeList<NeighbourNodeData>(Allocator.Temp);

					foreach (int2 offset in Pathfinding.Offsets)
					{
						int x = i + offset.x;
						int y = j + offset.y;

						if (x < 0 || x >= settings.Size.x || y < 0 || y >= settings.Size.y)
						{
							continue;
						}

						int neighbourId = y * settings.Size.x + x;

						NeighbourNodeData neighbourNode = new()
						{
							Id = neighbourId,
							HeuristicCost = Pathfinding.Heuristic(new float2(i, j), new float2(x, y))
						};

						tempNodes.Add(neighbourNode);
					}

					int id = j * settings.Size.x + i;
					nodes[id] = new()
					{
						Id = id,
						Coordinates = new(i - settings.Size.x / 2, j - settings.Size.y / 2),
						Neighbours = tempNodes.ToArray(Allocator.Persistent)
					};
				}
			}

			var penalties = new NativeArray<float>(nodesCount, Allocator.Persistent);

			var terrain = Object.FindFirstObjectByType<Terrain>();
			var terrainPathfindingData = terrain.GetComponent<TerrainPathfindingData>();

			var terrainData = terrain.terrainData;
			var alphaMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
			var alphaMapsCount = alphaMaps.GetLength(2);

			for (int i = 0; i < settings.Size.x; i++)
			{
				for (int j = 0; j < settings.Size.y; j++)
				{
					int x = (int)(i / terrainData.size.x * terrainData.alphamapWidth);
					int y = (int)(j / terrainData.size.z * terrainData.alphamapHeight);

					float penalty = 0;

					for (int k = 0; k < alphaMapsCount; k++)
					{
						float alpha = alphaMaps[y, x, k];
						float penaltyByLayer = terrainPathfindingData.PenaltiesByLayer[k];
						penalty += alpha * penaltyByLayer;
					}

					int id = j * settings.Size.x + i;
					penalties[id] = penalty;
				}
			}

			Map map = new()
			{
				Size = settings.Size,
				Nodes = nodes,
				Obstacles = new NativeArray<bool>(nodesCount, Allocator.Persistent),
				Penalties = penalties
			};

			EntityManager.SetComponentData(entity, map);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (SystemAPI.HasSingleton<Map>())
			{
				var map = SystemAPI.GetSingleton<Map>();

				foreach (var node in map.Nodes)
				{
					node.Neighbours.Dispose();
				}

				map.Nodes.Dispose();
				map.Obstacles.Dispose();
				map.Penalties.Dispose();
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

				map.ValueRW.Obstacles[i] = obs;
			}

			obstacles.Dispose();
		}
	}
}
