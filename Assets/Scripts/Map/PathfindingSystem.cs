using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct PathfindingSystem : ISystem
	{
		private struct Info : IHeapItem<Info>
		{
			public int Id { get; set; }

			public float GCost;
			public float HCost;
			public float FCost;

			public int HeapIndex { get; set; }

			public int CompareTo(Info other)
			{
				int compare = FCost.CompareTo(other.FCost);
				if (compare == 0)
				{
					compare = HCost.CompareTo(other.HCost);
				}
				return -compare;
			}
		}

		private NativeArray<int> parents;
		private NativeHashSet<int> closedNodes;
		private Heap<Info> openNodes;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			var mapSettigns = SystemAPI.GetSingleton<MapSettings>();

			int nodesCount = mapSettigns.Size.x * mapSettigns.Size.y;

			parents = new NativeArray<int>(nodesCount, Allocator.Persistent);
			closedNodes = new NativeHashSet<int>(nodesCount, Allocator.Persistent);
			openNodes = new Heap< Info>(nodesCount);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{
			parents.Dispose();
			closedNodes.Dispose();
			openNodes.Dispose();
		}


		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var map = SystemAPI.GetSingleton<Map>();

			foreach ((var localTransform, var waypointBuffer, var unitInput) in SystemAPI.Query<LocalTransform, DynamicBuffer<Waypoint>, UnitInput>().WithAll<Simulate>())
			{
				waypointBuffer.Clear();

				Node start = map.NodeFromPosition(localTransform.Position);
				Node goal = map.NodeFromPosition(unitInput.Position);

				var s = new Stopwatch();
				s.Start();
				var nodes = FindPath(map, start, goal);
				s.Stop();

				UnityEngine.Debug.Log($"{s.ElapsedTicks / (float)Stopwatch.Frequency * 1000} ms");

				if (nodes != null)
				{
					foreach (var node in nodes)
					{
						Waypoint waypoint = new()
						{
							Position = new float3(node.Coordinates.x + 0.5f, 0, node.Coordinates.y + 0.5f)
						};

						waypointBuffer.Add(waypoint);
					}
				}
			}
		}

		public List<Node> ReconstructPath(Map map, Node current)
		{
			var path = new List<Node> { current };

			while (parents[current.Id] != -1)
			{
				int currentId = parents[current.Id];
				current = map.Nodes[currentId];
				path.Add(current);
			}

			path.Reverse();

			path.RemoveAt(0);

			return path;
		}

		private void Add(Node start, Node goal, float gCost)
		{
			var info = new Info
			{
				Id = start.Id,
				GCost = gCost,
				HCost = Pathfinding.Heuristic(start.Coordinates, goal.Coordinates),
			};

			info.FCost = info.GCost + info.HCost;

			openNodes.Add(info);
			parents[start.Id] = -1;
		}

		public List<Node> FindPath(Map map, Node start, Node goal)
		{
			openNodes.Clear();
			closedNodes.Clear();

			Add(start, goal, 0);

			while (openNodes.Count > 0)
			{
				Info currentInfo = openNodes.RemoveFirst();
				int currentId = currentInfo.Id;
				Node currentNode = map.Nodes[currentId];

				if (currentId == goal.Id)
				{
					return ReconstructPath(map, currentNode);
				}

				float currentGCost = currentInfo.GCost;

				closedNodes.Add(currentId);

				foreach (NeighbourNodeData neighbourNode in map.Nodes[currentId].Neighbours)
				{
					Node node = map.Nodes[neighbourNode.Id];
					int neighbourId = neighbourNode.Id;

					if (map.Obstacles[neighbourId])
					{
						continue;
					}

					if (closedNodes.Contains(neighbourNode.Id))
					{
						continue;
					}

					float tentativeGCost = currentGCost + neighbourNode.HeuristicCost * map.Penalties[neighbourId];

					if (!openNodes.Contains(neighbourId))
					{
						Add(node, goal, tentativeGCost);
						parents[neighbourId] = currentId;
					}
					else
					{
						Info info = openNodes.Get(neighbourId);

						if (tentativeGCost < info.GCost)
						{
							info.GCost = tentativeGCost;
							info.FCost = info.GCost + info.HCost;
							openNodes.SortUp(info);
							parents[neighbourId] = currentId;
						}
					}
				}
			}

			return null;
		}
	}
}
