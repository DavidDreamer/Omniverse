using Dreambox.Math;

namespace Omniverse.FogOfWar
{
	internal readonly struct FogOfWarCircleHandler: IBresenhamCircleHandler
	{
		private int X { get; }

		private int Y { get; }

		private Cell[,] Cells { get; }
			
		public FogOfWarCircleHandler(int x, int y, Cell[,] cells)
		{
			X = x;
			Y = y;

			Cells = cells;
		}

		public void HandlePoint(int x, int y)
		{
			var lineHandler = new FogOfWarLineHandler(Cells);
			Bresenham.Line(X, Y, x, y, lineHandler);
		}
	}
}
