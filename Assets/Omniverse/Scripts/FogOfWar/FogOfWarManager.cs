using System.Collections.Generic;
using Dreambox.Math;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Omniverse.FogOfWar
{
	public class FogOfWarManager: IInitializable, IFixedTickable
	{
		public static int Multiplier { get; } = (int)Mathf.Pow(2f, 1f);

		public Vector2Int Resolution { get; set; }

		[Inject]
		private GameSettings GameSettings { get; set; }

		[Inject]
		public Player Player { get; set; }

		public Dictionary<int, Cell[,]> Cells { get; set; } = new();

		public void Initialize()
		{
			Resolution = GameSettings.MapSettings.Size / Multiplier;

			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;

			for (int i = 0; i < GameSettings.Factions.Length; ++i)
			{
				Cell[,] cells = new Cell[Resolution.x, Resolution.y];
				for (int x = 0; x < Resolution.x; ++x)
				{
					for (int y = 0; y < Resolution.y; ++y)
					{
						cells[x, y] = new Cell
						{
							Position = new Vector3(x, 0, y) * Multiplier + offset,
							VisibilityState = CellVisibilityState.Concealed,
							Neighbours = new()
						};
					}
				}

				for (int x = 0; x < Resolution.x; ++x)
				{
					for (int y = 0; y < Resolution.y; ++y)
					{
						var cell = cells[x, y];
						if (x > 0)
						{
							cell.Neighbours.Add(cells[x - 1, y]);
						}

						if (x < Resolution.x - 1)
						{
							cell.Neighbours.Add(cells[x + 1, y]);
						}

						if (y > 0)
						{
							cell.Neighbours.Add(cells[x, y - 1]);
						}

						if (y < Resolution.y - 1)
						{
							cell.Neighbours.Add(cells[x, y + 1]);
						}
					}
				}

				Cells.Add(i, cells);
			}
		}

		public float delta = 0.03f;

		private Queue<Cell> Queue { get; } = new();

		private HashSet<Cell> Set { get; } = new();

		public void AddObstacle(FogOfWarObstacle obstacle)
		{
			foreach (var pair in Cells)
			{
				var cellIndex = CalculateCell(obstacle.transform);
				Cell cell = pair.Value[cellIndex.x, cellIndex.y];
				
				Queue.Clear();
				Set.Clear();

				Queue.Enqueue(cell);
				Set.Add(cell);

				while (Queue.Count > 0)
				{
					cell = Queue.Dequeue();

					Vector3 cellPosition = obstacle.transform.InverseTransformPoint(cell.Position);

					bool contains = Mathf.Abs(cellPosition.x) <= obstacle.Size.x / 2f &&
					                Mathf.Abs(cellPosition.z) <= obstacle.Size.z / 2f;
					if (!contains)
					{
						continue;
					}

					cell.Occluded = true;

					foreach (Cell neighbourCell in cell.Neighbours)
					{
						if (!Set.Contains(neighbourCell))
						{
							Set.Add(neighbourCell);
							Queue.Enqueue(neighbourCell);
						}
					}
				}
			}
		}

		private Vector2Int CalculateCell(Transform transform)
		{
			Vector3 position = transform.position;

			int x = (int)position.x / Multiplier;
			int y = (int)position.z / Multiplier;

			return new Vector2Int(x, y);
		}

		private FogOfWarAgent[] Agents { get; set; }

		private void UpdateAgentPositions()
		{
			Agents = Object.FindObjectsOfType<FogOfWarAgent>();

			foreach (FogOfWarAgent agent in Agents)
			{
				agent.Cell = CalculateCell(agent.transform);
			}
		}

		private void UpddateAgentsVisibility()
		{
			foreach (FogOfWarAgent agent in Agents)
			{
				int factionID = Player.FactionID;
				Vector2Int cellIndex = agent.Cell;
				CellVisibilityState cellVisibilityState = Cells[factionID][cellIndex.x, cellIndex.y].VisibilityState;
				agent.GetComponentInChildren<UnitRendererBase>(true).gameObject
					.SetActive(cellVisibilityState is CellVisibilityState.Visible);
			}
		}

		private void Clear()
		{
			foreach (var pair in Cells)
			{
				for (int x = 0; x < Resolution.x; ++x)
				{
					for (int y = 0; y < Resolution.y; ++y)
					{
						pair.Value[x, y].VisibilityState = CellVisibilityState.Concealed;
					}
				}
			}
		}

		private void Animate()
		{
			foreach (var pair in Cells)
			{
				for (int x = 0; x < Resolution.x; ++x)
				{
					for (int y = 0; y < Resolution.y; ++y)
					{
						Cell cell = pair.Value[x, y];

						cell.Value += (cell.VisibilityState == CellVisibilityState.Visible ? -1 : 1) * delta;
						cell.Value = Mathf.Clamp01(cell.Value);
					}
				}
			}
		}

		private void CalculateVisibility()
		{
			foreach (FogOfWarAgent agent in Agents)
			{
				int x0 = (int)agent.transform.position.x / Multiplier;
				int y0 = (int)agent.transform.position.z / Multiplier;
				int radius = (int)agent.Range / Multiplier;
				int factionId = agent.FactionID;
				
				var circleHandler = new FogOfWarCircleHandler(x0, y0, Cells[factionId]);
				Bresenham.Circle(x0, y0, radius, circleHandler);
			}
		}

		public void FixedTick()
		{
			UpdateAgentPositions();
			Clear();

			CalculateVisibility();

			Animate();

			UpddateAgentsVisibility();
		}
	}
}
