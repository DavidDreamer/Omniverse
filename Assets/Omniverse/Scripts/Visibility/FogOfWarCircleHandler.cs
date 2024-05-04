using Dreambox.Math;

namespace Omniverse.Visibility
{
	internal readonly struct FogOfWarCircleHandler: IBresenhamCircleHandler
	{
		private int X { get; }

		private int Y { get; }

		private FogOfWar FogOfWar { get; }
			
		public FogOfWarCircleHandler(int x, int y, FogOfWar fogOfWar)
		{
			X = x;
			Y = y;

			FogOfWar = fogOfWar;
		}

		public void HandlePoint(int x, int y)
		{
			var lineHandler = new FogOfWarLineHandler(FogOfWar);
			Bresenham.Line(X, Y, x, y, lineHandler);
		}
	}
}
