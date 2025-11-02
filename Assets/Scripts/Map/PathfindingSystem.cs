using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		private struct Info
		{
			public float GCost;
			public float HCost;
			public float FCost => GCost + HCost;
			public int Parent;
		}

		private NativeList<int> openNodes;
		private NativeHashSet<int> closedNodes;
		private NativeHashMap<int, Info> nodesInfo;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			var mapSettigns = SystemAPI.GetSingleton<MapSettings>();

			int nodesCount = mapSettigns.Size.x * mapSettigns.Size.y;

			openNodes = new NativeList<int>(nodesCount, Allocator.Persistent);
			closedNodes = new NativeHashSet<int>(nodesCount, Allocator.Persistent);
			nodesInfo = new NativeHashMap<int, Info>(nodesCount, Allocator.Persistent);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{
			openNodes.Dispose();
			closedNodes.Dispose();
			nodesInfo.Dispose();
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

			while (nodesInfo[current.Id].Parent != -1)
			{
				int currentId = nodesInfo[current.Id].Parent;
				current = map.Nodes[currentId];
				path.Add(current);
			}

			path.Reverse();

			path.RemoveAt(0);

			return path;
		}

		private int GetSmallestGScore()
		{
			int id = openNodes[0];
			float smallestScore = nodesInfo[id].FCost;
			int openNodeIndex = 0;

			for (int i = 1; i < openNodes.Length; i++)
			{
				int currentId = openNodes[i];
				float currentGScore = nodesInfo[currentId].FCost;

				if (currentGScore < smallestScore)
				{
					smallestScore = currentGScore;
					id = currentId;
					openNodeIndex = i;
				}
			}

			openNodes.RemoveAt(openNodeIndex);

			return id;
		}

		public List<Node> FindPath(Map map, Node start, Node goal)
		{
			openNodes.Clear();
			closedNodes.Clear();
			nodesInfo.Clear();

			openNodes.Add(start.Id);
			nodesInfo.Add(start.Id, new Info
			{
				GCost = 0,
				HCost = Pathfinding.Heuristic(start.Coordinates, goal.Coordinates),
				Parent = -1
			});
		
			while (openNodes.Length > 0)
			{
				int currentId = GetSmallestGScore();
				Node currentNode = map.Nodes[currentId];

				if (currentId == goal.Id)
				{
					return ReconstructPath(map, currentNode);
				}

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

					float tentativeGScore = nodesInfo[currentId].GCost + neighbourNode.HeuristicCost * map.Passability[neighbourId];

					if (!nodesInfo.ContainsKey(neighbourId))
					{
						Info info = new()
						{
							GCost = tentativeGScore,
							HCost = Pathfinding.Heuristic(node.Coordinates, goal.Coordinates),
							Parent = currentId
						};

						nodesInfo.Add(neighbourId, info);
					}
					
					if (tentativeGScore < nodesInfo[neighbourNode.Id].GCost)
					{
						Info info = nodesInfo[neighbourId];
						info.GCost = tentativeGScore;
						info.Parent = currentId;
						nodesInfo[neighbourId] = info;
					}

					if (!openNodes.Contains(neighbourNode.Id))
					{
						openNodes.Add(neighbourNode.Id);
					}
				}
			}

			return null;
		}
	}
}
