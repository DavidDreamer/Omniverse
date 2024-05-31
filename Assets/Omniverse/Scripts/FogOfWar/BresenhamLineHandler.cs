using Dreambox.Math;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	internal readonly struct BresenhamLineHandler: IBresenhamLineHandler
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

			if (index < 0 || index >= CellVisibilityStates.Length)
			{
				return true;
			}
			
			CellVisibilityStates[index] = CellVisibilityState.Visible;

			return CellObstacles[index];
		}
	}
}
