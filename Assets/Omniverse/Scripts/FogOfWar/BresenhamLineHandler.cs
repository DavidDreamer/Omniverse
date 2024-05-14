using Dreambox.Math;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	internal readonly struct BresenhamLineHandler: IBresenhamLineHandler
	{
		private Cell[] Cells { get; }

		private Vector2Int Resolution { get; }
		
		public BresenhamLineHandler(Cell[] cells, Vector2Int resoution)
		{
			Cells = cells;
			Resolution = resoution;
		}
			
		public bool HandlePoint(int x, int y)
		{
			Cell cell = Cells[x * Resolution.y + y];

			if (cell.Occluded)
			{
				return true;
			}

			cell.VisibilityState = CellVisibilityState.Visible;

			return false;
		}
	}
}
