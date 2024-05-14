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
		private MapSettings MapSettings { get; set; }

		[Inject]
		private FactionDesc[] Factions { get; set; }
		
		[Inject]
		public Player Player { get; set; }

		public Dictionary<int, Cell[,]> Cells { get; set; } = new();

		public void Initialize()
		{
			Resolution = MapSettings.Size / Multiplier;

			for (int i = 0; i < Factions.Length; ++i)
			{
				Cell[,] cells = new Cell[Resolution.x, Resolution.y];
				for (int x = 0; x < Resolution.x; ++x)
				{
					for (int y = 0; y < Resolution.y; ++y)
					{
						cells[x, y] = new Cell
						{
							VisibilityState = CellVisibilityState.Concealed
						};
					}
				}

				Cells.Add(i, cells);
			}
		}

		public static Vector3 CalculateCellCenter(int x, int y)
		{
			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;
			
			return new Vector3(x, 0, y) * Multiplier + offset;
		}
		
		public void AddObstacle(FogOfWarObstacle obstacle)
		{
			foreach (var pair in Cells)
			{
				for (var x = 0; x < Resolution.x; x++)
				for (var y = 0; y < Resolution.y; y++)
				{
					Cell cell = pair.Value[x, y];
					Vector3 cellCenter = CalculateCellCenter(x, y);
					Vector3 cellPosition = obstacle.transform.InverseTransformPoint(cellCenter);

					cell.Occluded |= Mathf.Abs(cellPosition.x) <= obstacle.Size.x / 2f &&
					                 Mathf.Abs(cellPosition.z) <= obstacle.Size.z / 2f;
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

			UpddateAgentsVisibility();
		}
	}
}
