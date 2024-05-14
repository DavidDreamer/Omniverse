using Dreambox.Math;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	internal readonly struct BresenhamCircleHandler: IBresenhamCircleHandler
	{
		private int X { get; }

		private int Y { get; }

		private Cell[] Cells { get; }
			
		private Vector2Int Resolution { get; }

		public BresenhamCircleHandler(int x, int y, Cell[] cells, Vector2Int resoution)
		{
			X = x;
			Y = y;

			Cells = cells;
			Resolution = resoution;
		}

		public void HandlePoint(int x, int y)
		{
			var lineHandler = new BresenhamLineHandler(Cells, Resolution);
			Bresenham.Line(X, Y, x, y, lineHandler);
		}
	}
}
