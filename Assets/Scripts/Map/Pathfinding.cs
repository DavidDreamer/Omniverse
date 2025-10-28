using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Omniverse
{
	public static class Pathfinding
	{
		public static int2[] Offsets = new int2[8]
		{
			new(-1, 1),
			new(0, 1),
			new(1, 1),
			new(-1, 0),
			new(1, 0),
			new(-1, -1),
			new(0, -1),
			new(1, -1),
		};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Heuristic(float2 from, float2 to) => math.distancesq(to, from);
	}
}
