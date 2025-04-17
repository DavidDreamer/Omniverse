using Unity.Mathematics;

namespace Omniverse
{
	public static class FogOfWarUtils
	{
		public static int2 CellCoordinatesFromPosition(this FogOfWar fogOfWar, float3 position, int2 mapSize)
		{
			int2 coordinates = new((int)position.x, (int)position.z);
			coordinates += mapSize / 2;
			coordinates /= FogOfWar.Multiplier;
			return coordinates;
		}

		public static int CellIndexFromPosition(this FogOfWar fogOfWar, float3 position, int2 mapSize)
		{
			int2 coordinates = fogOfWar.CellCoordinatesFromPosition(position, mapSize);
			return coordinates.x * fogOfWar.Size.y + coordinates.y;
		}
	}
}
