using System.Collections.Generic;
using Dreambox.Math;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Omniverse.Visibility
{
	public class FogOfWarCell
	{
		public List<FogOfWarCell> Neighbours;

		public bool Occluded;

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

		private Queue<FogOfWarCell> Queue { get; } = new();

		private HashSet<FogOfWarCell> Set { get; } = new();

		public void AddObstacle(FogOfWarObstacle obstacle)
		{
			FogOfWarCell cell = CalculateCell(obstacle.transform);

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

				foreach (FogOfWarCell neighbourCell in cell.Neighbours)
				{
					if (!Set.Contains(neighbourCell))
					{
						Set.Add(neighbourCell);
						Queue.Enqueue(neighbourCell);
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
				agent.GetComponentInChildren<UnitRendererBase>(true).gameObject
					.SetActive(agent.Cell.Reveled[Player.FactionID]);
			}
		}

		private void Clear()
		{
			for (int x = 0; x < Resolution.x; ++x)
			{
				for (int y = 0; y < Resolution.y; ++y)
				{
					FogOfWarCell cell = Cells[x, y];

					for (var i = 0; i < cell.Reveled.Length; i++)
					{
						cell.Reveled[i] = false;
					}
				}
			}
		}

		private void Animate()
		{
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
		}

		public struct S: IBresenhamLineHandler
		{
			public FogOfWar FogOfWar;
			
			public bool Invoke(int x, int y)
			{
				FogOfWarCell cell = FogOfWar.Cells[x, y];

				if (cell.Occluded)
				{
					return true;
				}

				cell.Reveled[0] = true;

				return false;
			}
		}
		
		private void CalculateVisibility(int x0, int y0, int x1, int y1, int faction = 0)
		{
			var s = new S
			{
				FogOfWar = this
			};

			Bresenham.Line(x0, y0, x1, y1, s);
		}

		private void Calc(int x0, int y0, int radius)
		{
			int x = radius;
			int y = 0;
			int radiusError = 1 - x;

			while (x >= y)
			{
				CalculateVisibility(x0, y0, x0 + x, y0 + y);
				CalculateVisibility(x0, y0, x0 + x, y0 - y);
				CalculateVisibility(x0, y0, x0 - x, y0 + y);
				CalculateVisibility(x0, y0, x0 - x, y0 - y);
				CalculateVisibility(x0, y0, x0 + y, y0 + x);
				CalculateVisibility(x0, y0, x0 + y, y0 - x);
				CalculateVisibility(x0, y0, x0 - y, y0 + x);
				CalculateVisibility(x0, y0, x0 - y, y0 - x);

				if (true)
				{
					CalculateVisibility(x0, y0, x0 + x + 1, y0 + y);
					CalculateVisibility(x0, y0, x0 + x + 1, y0 - y);
					CalculateVisibility(x0, y0, x0 - x + 1, y0 + y);
					CalculateVisibility(x0, y0, x0 - x + 1, y0 - y);
					CalculateVisibility(x0, y0, x0 + y + 1, y0 + x);
					CalculateVisibility(x0, y0, x0 + y + 1, y0 - x);
					CalculateVisibility(x0, y0, x0 - y + 1, y0 + x);
					CalculateVisibility(x0, y0, x0 - y + 1, y0 - x);
				}

				y++;

				if (radiusError < 0)
				{
					radiusError += 2 * y + 1;
				}
				else
				{
					x--;
					radiusError += 2 * (y - x + 1);
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

				Calc(x0, y0, radius);
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
