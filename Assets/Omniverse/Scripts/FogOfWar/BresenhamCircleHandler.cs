using Dreambox.Math;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	internal readonly struct BresenhamCircleHandler: IBresenhamCircleHandler
	{
		private int X { get; }

		private int Y { get; }

		private CellVisibilityState[] CellVisibilityStates { get; }

		private bool[] CellObstacles { get; }
			
		private Vector2Int Resolution { get; }

		public BresenhamCircleHandler(int x, int y, CellVisibilityState[] cellVisibilityStates, bool[] cellObstacles, Vector2Int resoution)
		{
			X = x;
			Y = y;

			CellVisibilityStates = cellVisibilityStates;
			CellObstacles = cellObstacles;
			Resolution = resoution;
		}

		public void HandlePoint(int x, int y)
		{
			var lineHandler = new BresenhamLineHandler(CellVisibilityStates, CellObstacles, Resolution);
			Bresenham.Line(X, Y, x, y, lineHandler);
		}
	}
}
