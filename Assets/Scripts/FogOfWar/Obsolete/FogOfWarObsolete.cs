using System;
using System.Collections.Generic;
using Dreambox.Math;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[Obsolete]
	public partial class FogOfWarObsolete : IInitializable
	{
		public static int Multiplier { get; } = (int)Mathf.Pow(2f, 1f);

		public Vector2Int Resolution { get; set; }

		[Inject]
		public FogOfWarConfig Config { get; set; }

		[Inject]
		private MapSettings MapSettings { get; set; }

		[Inject]
		private FactionDesc[] Factions { get; set; }

		[Inject]
		public Player Player { get; set; }

		public CellVisibilityState[][] CellsVisibilityPerFaction { get; set; }

		public bool[][] CellsObstaclesPerFaction { get; set; }

		private HashSet<IFogOfWarAgentObsolete> Agents { get; } = new();

		public void Initialize()
		{
			Resolution = MapSettings.Size / Multiplier;
			int cellsCount = Resolution.x * Resolution.y;

			CellsVisibilityPerFaction = new CellVisibilityState[Factions.Length][];
			CellsObstaclesPerFaction = new bool[Factions.Length][];

			CellVisibilityState initialState =
				Config.Explored ? CellVisibilityState.Explored : CellVisibilityState.Unexplored;

			for (int i = 0; i < Factions.Length; ++i)
			{
				CellsVisibilityPerFaction[i] = new CellVisibilityState[cellsCount];
				CellsObstaclesPerFaction[i] = new bool[cellsCount];

				for (int j = 0; j < cellsCount; ++j)
				{
					CellsVisibilityPerFaction[i][j] = initialState;
				}
			}
		}

		public static Vector3 CalculateCellCenter(int x, int y)
		{
			Vector3 size = new Vector3(1, 0, 1) * Multiplier;
			Vector3 offset = size / 2f;

			return new Vector3(x, 0, y) * Multiplier + offset;
		}

		//TODO
		public void AddObstacle(FogOfWarObstacleObsolete obstacle)
		{
			int x = (int)obstacle.transform.position.x / Multiplier;
			int y = (int)obstacle.transform.position.z / Multiplier;

			int index = x * Resolution.y + y;
			foreach (bool[] cells in CellsObstaclesPerFaction)
			{
				cells[index] = true;
			}
		}

		private int CalculateCell(Vector3 position)
		{
			int x = (int)position.x / Multiplier;
			int y = (int)position.z / Multiplier;

			return x * Resolution.y + y;
		}

		public void Register(IFogOfWarAgentObsolete agent)
		{
			Agents.Add(agent);
		}

		public void Unregister(IFogOfWarAgentObsolete agent)
		{
			Agents.Remove(agent);
		}

		private void UpdateAgentPositions()
		{
			foreach (IFogOfWarAgentObsolete agent in Agents)
			{
				agent.CellIndex = CalculateCell(agent.Position);
			}
		}

		private void Clear()
		{
			if (Config.Explored)
			{
				foreach (var cells in CellsVisibilityPerFaction)
				{
					for (var index = 0; index < cells.Length; index++)
					{
						cells[index] = CellVisibilityState.Explored;
					}
				}
			}
			else
			{
				foreach (var cells in CellsVisibilityPerFaction)
				{
					for (var index = 0; index < cells.Length; index++)
					{
						if (cells[index] is CellVisibilityState.Visible)
						{
							cells[index] = CellVisibilityState.Explored;
						}
					}
				}
			}
		}

		private void CalculateVisibility()
		{
			foreach (IFogOfWarAgentObsolete agent in Agents)
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

		public void Tick()
		{
			UpdateAgentPositions();
			Clear();
			CalculateVisibility();
		}
	}
}
