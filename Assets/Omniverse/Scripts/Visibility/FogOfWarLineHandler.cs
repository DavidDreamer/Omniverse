using Dreambox.Math;

namespace Omniverse.Visibility
{
	internal readonly struct FogOfWarLineHandler: IBresenhamLineHandler
	{
		private Cell[,] Cells { get; }

		public FogOfWarLineHandler(Cell[,] cells)
		{
			Cells = cells;
		}
			
		public bool HandlePoint(int x, int y)
		{
			Cell cell = Cells[x, y];

			if (cell.Occluded)
			{
				return true;
			}

			cell.VisibilityState = CellVisibilityState.Visible;

			return false;
		}
	}
}
