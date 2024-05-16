using System.Collections.Generic;
using Dreambox.Math;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.FogOfWar
{
	public class Manager: IInitializable, IFixedTickable
	{
		public static int Multiplier { get; } = (int)Mathf.Pow(2f, 1f);

		public Vector2Int Resolution { get; set; }

		[Inject]
		private MapSettings MapSettings { get; set; }

		[Inject]
		private FactionDesc[] Factions { get; set; }
		
		[Inject]
		public IPlayer Player { get; set; }

		public CellVisibilityState[][] CellsVisibilityPerFaction { get; set; }

		public bool[][] CellsObstaclesPerFaction { get; set; }

		private HashSet<IAgent> Agents { get; } = new();
		
		public void Initialize()
		{
			Resolution = MapSettings.Size / Multiplier;
			int cellsCount = Resolution.x * Resolution.y;

			CellsVisibilityPerFaction = new CellVisibilityState[Factions.Length][];
			CellsObstaclesPerFaction = new bool[Factions.Length][];
			
			for (int i = 0; i < Factions.Length; ++i)
			{
				CellsVisibilityPerFaction[i] = new CellVisibilityState[cellsCount];
				CellsObstaclesPerFaction[i] = new bool[cellsCount];
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
			foreach (bool[] cells in CellsObstaclesPerFaction)
			{
				for (var x = 0; x < Resolution.x; x++)
				for (var y = 0; y < Resolution.y; y++)
				{
					int index = x * Resolution.y + y;
					Vector3 cellCenter = CalculateCellCenter(x, y);
					Vector3 cellPosition = obstacle.transform.InverseTransformPoint(cellCenter);

					cells[index] |= Mathf.Abs(cellPosition.x) <= obstacle.Size.x / 2f &&
					                Mathf.Abs(cellPosition.z) <= obstacle.Size.z / 2f;
				}
			}
		}

		private int CalculateCell(Vector3 position)
		{
			int x = (int)position.x / Multiplier;
			int y = (int)position.z / Multiplier;

			return x * Resolution.y + y;
		}

		public void Register(IAgent agent)
		{
			Agents.Add(agent);
		}
		
		public void Unregister(IAgent agent)
		{
			Agents.Remove(agent);
		}
		
		private void UpdateAgentPositions()
		{
			foreach (IAgent agent in Agents)
			{
				agent.CellIndex = CalculateCell(agent.Position);
			}
		}

		private void Clear()
		{
			foreach (var cells in CellsVisibilityPerFaction)
			{
				for (var index = 0; index < cells.Length; index++)
				{
					cells[index] = CellVisibilityState.Concealed;
				}
			}
		}

		private void CalculateVisibility()
		{
			foreach (IAgent agent in Agents)
			{
				int x0 = (int)agent.Position.x / Multiplier;
				int y0 = (int)agent.Position.z / Multiplier;
				int radius = (int)agent.VisionRange / Multiplier;
				int factionId = agent.FactionID;

				var circleHandler = new BresenhamCircleHandler(x0, y0, CellsVisibilityPerFaction[factionId],
					CellsObstaclesPerFaction[factionId], Resolution);
				Bresenham.Circle(x0, y0, radius, circleHandler);
			}
		}

		public void FixedTick()
		{
			UpdateAgentPositions();
			Clear();
			CalculateVisibility();
		}
	}
}
