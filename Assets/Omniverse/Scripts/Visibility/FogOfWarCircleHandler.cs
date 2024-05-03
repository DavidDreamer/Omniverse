using Dreambox.Math;

namespace Omniverse.Visibility
{
	internal readonly struct FogOfWarCircleHandler: IBresenhamCircleHandler
	{
		public int X { get; }
			
		public int Y { get; }

		public  FogOfWar FogOfWar { get; }
			
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
