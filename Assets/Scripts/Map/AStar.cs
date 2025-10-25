using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Omniverse
{
	public static class AStar
	{
		private struct Score
		{
			public float G;
			public float H;
			public float F => G + H;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float Heuristic(Node from, Node to) => math.distancesq(to.Coordinates, from.Coordinates);

		private static int2[] Offsets = new int2[8]
		{
			new(-1, 1),
			new(0, 1),
			new(1, 1),
			new(-1, 0),
			new(1, 0),
			new(-1, -1),
			new(0, -1),
			new(1, -1),
		};

		public static List<Node> ReconstructPath(Map map, NativeHashMap<int, int> parentMap, Node current)
		{
			var path = new List<Node> { current };

			while (parentMap.ContainsKey(current.Id))
			{
				int currentId = parentMap[current.Id];
				current = map.Nodes[currentId];
				path.Add(current);
			}

			path.Reverse();

			path.RemoveAt(0);

			return path;
		}

		public static List<Node> FindPath(Map map, Node start, Node goal)
		{
			var openList = new HashSet<int>(10);
			var closedList = new NativeHashSet<int>(10, Allocator.Temp);
			var scoreMap = new NativeHashMap<int, Score>(10, Allocator.Temp);
			var parentMap = new NativeHashMap<int, int>(10, Allocator.Temp);

			openList.Add(start.Id);
			scoreMap.Add(start.Id, new Score
			{
				G = 0,
				H = Heuristic(start, goal),
			});

			while (openList.Count > 0)
			{
				var currentId = openList.OrderBy(node => scoreMap[node].F).First();
				Node currentNode = map.Nodes[currentId];

				if (currentId == goal.Id)
				{
					return ReconstructPath(map, parentMap, currentNode);
				}

				openList.Remove(currentId);
				closedList.Add(currentId);

				foreach (int2 offset in Offsets)
				{
					int2 neighbourCoordinate = currentNode.Coordinates + offset;

					int halfX = map.Size.x / 2;
					int halfY = map.Size.y / 2;

					if (neighbourCoordinate.x < -halfX || neighbourCoordinate.x > (halfX +1 ) || neighbourCoordinate.y < - halfY || neighbourCoordinate.y > (halfY + 1))
					{
						continue;
					}

					int neighbourId = (neighbourCoordinate.y + (map.Size.y / 2)) * map.Size.x + (neighbourCoordinate.x + (map.Size.x / 2));
 					Node neighbourNode = map.Nodes[neighbourId];

					if (neighbourNode.Obstacle)
					{
						continue;
					}

					if (closedList.Contains(neighbourNode.Id))
					{
						continue;
					}

					float tentativeGScore = scoreMap[currentId].G + Heuristic(currentNode, neighbourNode);

					if (!scoreMap.ContainsKey(neighbourNode.Id) || tentativeGScore < scoreMap[neighbourNode.Id].G)
					{
						Score score = new()
						{
							G = tentativeGScore,
							H = Heuristic(neighbourNode, goal)
						};

						if (!scoreMap.ContainsKey(neighbourNode.Id))
						{
							scoreMap.Add(neighbourId, score);
						}
						else
						{
							scoreMap[neighbourId] = score;
						}
			
						scoreMap[neighbourNode.Id] = score;

						if (!parentMap.ContainsKey(neighbourNode.Id))
						{
							parentMap.Add(neighbourNode.Id, currentId);
						}
						else
						{
							parentMap[neighbourNode.Id] = currentId;
						}

						if (!openList.Contains(neighbourNode.Id))
						{
							openList.Add(neighbourNode.Id);
						}
					}
				}
			}

			//openList.Dispose();
			//closedList.Dispose();
			//scoreMap.Dispose();
			//parentMap.Dispose();

			return null;
		}
	}
}
