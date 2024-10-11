using Dreambox.Math;
using UnityEngine;

namespace Omniverse
{
	internal readonly struct BresenhamLineHandler : IBresenhamLineHandler
	{
		private CellVisibilityState[] CellVisibilityStates { get; }

		private bool[] CellObstacles { get; }

		private Vector2Int Resolution { get; }

		public BresenhamLineHandler(
			CellVisibilityState[] cellVisibilityStates,
			bool[] cellObstacles,
			Vector2Int resoution)
		{
			CellVisibilityStates = cellVisibilityStates;
			CellObstacles = cellObstacles;
			Resolution = resoution;
		}

		public bool HandlePoint(int x, int y)
		{
			int index = x * Resolution.y + y;

			CellVisibilityStates[index] = CellVisibilityState.Visible;

			return CellObstacles[index];
		}
	}
}
