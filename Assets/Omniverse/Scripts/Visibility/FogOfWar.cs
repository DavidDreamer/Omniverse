using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Visibility
{
	public class FogOfWarCell
	{
		public List<FogOfWarCell> Neighbours;

		public bool Occluded;

		public int[] Vision;
		
		public int[] Block;
		
		public Vector3 Position;

		public bool[] Reveled;

		public float Value;
	}

	public class FogOfWar: IInitializable, IFixedTickable
	{
		public static int Multiplier { get; } = (int)Mathf.Pow(2f, 1f);

		public Vector2Int Resolution { get; set; }

		[Inject]
		private GameSettings GameSettings { get; set; }

		[Inject]
		public Player Player { get; set; }

		public FogOfWarCell[,] Cells { get; set; }

		public void Initialize()
		{
			Resolution = GameSettings.MapSettings.Size / Multiplier;

			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;

			Cells = new FogOfWarCell[Resolution.x, Resolution.y];
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					Cells[x, y] = new FogOfWarCell
					{
						Position = new Vector3(x, 0, y) * Multiplier + offset,
						Reveled = new bool[GameSettings.Factions.Length],
						Vision = new int[GameSettings.Factions.Length],
						Block = new int[GameSettings.Factions.Length],
						Neighbours = new()
					};
				}
			}

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					var cell = Cells[x, y];
					if (x > 0)
					{
						cell.Neighbours.Add(Cells[x - 1, y]);
					}

					if (x < Resolution.x - 1)
					{
						cell.Neighbours.Add(Cells[x + 1, y]);
					}
					
					if (y > 0)
					{
						cell.Neighbours.Add(Cells[x, y - 1]);
					}

					if (y < Resolution.y - 1)
					{
						cell.Neighbours.Add(Cells[x, y + 1]);
					}
				}
			}
		}

		public float delta = 0.03f;

		struct Temp
		{
			public FogOfWarCell Cell;

			public int Steps;
		}
		
		private Queue<Temp> Queue { get; } = new();
		private HashSet<FogOfWarCell> Set { get; } = new();
		private HashSet<FogOfWarCell> Set2 { get; } = new();

		public void AddObstacle(FogOfWarObstacle obstacle)
		{
			FogOfWarCell cell = CalculateCell(obstacle.transform);
			
			Queue.Clear();
			Set.Clear();

			var temp = new Temp
			{
				Cell = cell,
				Steps = 0
			};

			Queue.Enqueue(temp);
			Set.Add(cell);

			while (Queue.Count > 0)
			{
				cell = Queue.Dequeue().Cell;

				Vector3 cellPosition = obstacle.transform.InverseTransformPoint(cell.Position);

				bool contains = Mathf.Abs(cellPosition.x) <= obstacle.Size.x / 2f &&
				                Mathf.Abs(cellPosition.z) <= obstacle.Size.z / 2f;
				if (!contains)
				{
					continue;
				}
				
				cell.Occluded = true;

				foreach (FogOfWarCell neighbourCell in cell.Neighbours)
				{
					if (!Set.Contains(neighbourCell))
					{
						Set.Add(neighbourCell);
						Queue.Enqueue(new Temp(){Cell = neighbourCell});
					}
				}
			}
		}

		private FogOfWarCell CalculateCell(Transform transform)
		{
			Vector3 position = transform.position;

			int x = (int)position.x / Multiplier;
			int y = (int)position.z / Multiplier;

			return Cells[x, y];
		}
		
		public void FixedTick()
		{
			var agents = Object.FindObjectsOfType<FogOfWarAgent>();

			foreach (FogOfWarAgent agent in agents)
			{
				agent.Cell = CalculateCell(agent.transform);
			}

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					for (var i = 0; i < cell.Reveled.Length; i++)
					{
						cell.Reveled[i] = false;
						cell.Vision[i] = 0;
						cell.Block[i] = 0;
					}
				}
			}
			
			foreach (FogOfWarAgent agent in agents)
			{
				float range = agent.Range * agent.Range;

				Queue.Clear();
				Set.Clear();
				Set2.Clear();

				Set2.Add(agent.Cell);
				agent.Cell.Vision[agent.FactionID]++;
				
				var temp = new Temp
				{
					Cell = agent.Cell,
					Steps = 1
				};

				//agent.Cell.Vision[agent.FactionID] = 1;
				
				Queue.Enqueue(temp);

				while (Queue.Count > 0)
				{
					var cell = Queue.Dequeue();

					Set.Add(cell.Cell);
					
					float distance = (cell.Cell.Position - agent.Cell.Position).sqrMagnitude;
					
					if (distance > range)
					{
						continue;
					}
					
					bool occluded = cell.Cell.Occluded ||
					                cell.Cell.Block[agent.FactionID] > cell.Cell.Vision[agent.FactionID];
					cell.Cell.Reveled[agent.FactionID] |= !occluded;

					if (cell.Cell.Occluded)
					{
						cell.Cell.Block[agent.FactionID] = cell.Steps;
					}
					
					if (occluded)
					{
						cell.Cell.Block[agent.FactionID]++;
						cell.Cell.Vision[agent.FactionID] = 0;
					}
					else
					{
						cell.Cell.Vision[agent.FactionID]++;
					}
					
					// float stepdistance = cell.Steps * cell.Steps * Multiplier;
					//
					// if (stepdistance > distance)
					// {
					// 	continue;
					// }
					
					//cell.Cell.Reveled[agent.FactionID] = true;

					foreach (FogOfWarCell neighbourCell in cell.Cell.Neighbours)
					{
						if (Set.Contains(neighbourCell))
						{
							continue;
						}
						
						if (!Set2.Contains(neighbourCell))
						{
							Set2.Add(neighbourCell);
							
							temp = new Temp
							{
								Cell = neighbourCell,
								Steps = cell.Steps + 1
							};
							
							Queue.Enqueue(temp);	
						}

						neighbourCell.Block[agent.FactionID] += cell.Cell.Block[agent.FactionID];
						neighbourCell.Vision[agent.FactionID] += cell.Cell.Vision[agent.FactionID];
					}
				}
			}

			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					//cell.Reveled[Player.FactionID] = Set.Contains(cell) && (!cell.Occluded || cell.Block[Player.FactionID] > cell.Vision[Player.FactionID]);
					
					cell.Value += (cell.Reveled[Player.FactionID] ? -1 : 1) * delta;
					cell.Value = Mathf.Clamp01(cell.Value);
				}
			}

			foreach (FogOfWarAgent agent in agents)
			{
				agent.GetComponentInChildren<UnitRendererBase>(true).gameObject
					.SetActive(agent.Cell.Reveled[Player.FactionID]);
			}
		}
	}
}
