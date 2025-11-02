using System;
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

		/// <summary>
		/// Octile distance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Heuristic(float2 from, float2 to)
		{
			float dx = Math.Abs(from.x - to.x);
			float dy = Math.Abs(from.y - to.y);

			float minDistance, maxDistance;

			if (dx >= dy)
			{
				minDistance = dy;
				maxDistance = dx;
			}
			else
			{
				minDistance = dx;
				maxDistance = dy;
			}

			const float cardinalCost = 1f;
			const float diagonalCost = 1.4f;

			return minDistance * diagonalCost + (maxDistance - minDistance) * cardinalCost;
		}
	}
}
