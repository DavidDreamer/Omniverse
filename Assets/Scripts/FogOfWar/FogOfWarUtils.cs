using Unity.Mathematics;

namespace Omniverse
{
	public static class FogOfWarUtils
	{
		public static int2 CellCoordinatesFromPosition(float3 position, int2 mapSize)
		{
			int2 coordinates = new((int)position.x, (int)position.z);
			coordinates += mapSize / 2;
			coordinates /= FogOfWar.Multiplier;
			return coordinates;
		}

		public static int CellIndexFromPosition(float3 position, int2 mapSize, int2 fogOfWarSize)
		{
			int2 coordinates = CellCoordinatesFromPosition(position, mapSize);
			return coordinates.x * fogOfWarSize.y + coordinates.y;
		}
	}
}
