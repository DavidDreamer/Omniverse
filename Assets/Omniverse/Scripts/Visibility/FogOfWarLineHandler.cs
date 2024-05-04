using Dreambox.Math;

namespace Omniverse.Visibility
{
	internal readonly struct FogOfWarLineHandler: IBresenhamLineHandler
	{
		private FogOfWar FogOfWar { get; }

		public FogOfWarLineHandler(FogOfWar fogOfWar)
		{
			FogOfWar = fogOfWar;
		}
			
		public bool HandlePoint(int x, int y)
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
}
