using Dreambox.Math;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public readonly struct BresenhamCircleHandler : IBresenhamCircleHandler
	{
		private int X { get; }

		private int Y { get; }

		private NativeArray<CellVisibilityState> CellVisibilityStates { get; }

		private NativeArray<bool> CellObstacles { get; }

		private int2 Size { get; }

		public BresenhamCircleHandler(int x, int y, NativeArray<CellVisibilityState> cellVisibilityStates, NativeArray<bool> cellObstacles, int2 size)
		{
			X = x;
			Y = y;

			CellVisibilityStates = cellVisibilityStates;
			CellObstacles = cellObstacles;
			Size = size;
		}

		public void HandlePoint(int x, int y)
		{
			x = Mathf.Clamp(x, 0, Size.x - 1);
			y = Mathf.Clamp(y, 0, Size.y - 1);

			var lineHandler = new BresenhamLineHandler(CellVisibilityStates, CellObstacles, Size);
			Bresenham.Line(X, Y, x, y, lineHandler);
		}
	}

	public readonly struct BresenhamLineHandler : IBresenhamLineHandler
	{
		private NativeArray<CellVisibilityState> CellVisibilityStates { get; }
		private CellVisibilityState[] CellVisibilityStatesArray { get; }

		private NativeArray<bool> CellObstacles { get; }

		private int2 Size { get; }

		public BresenhamLineHandler(
			NativeArray<CellVisibilityState> cellVisibilityStates,
			NativeArray<bool> cellObstacles,
			int2 size)
		{
			CellVisibilityStates = cellVisibilityStates;
			CellVisibilityStatesArray = CellVisibilityStates.ToArray();
			CellObstacles = cellObstacles;
			Size = size;
		}

		public bool HandlePoint(int x, int y)
		{
			int index = x * Size.y + y;

			CellVisibilityStatesArray[index] = CellVisibilityState.Visible;
			CellVisibilityStates.CopyFrom(CellVisibilityStatesArray);

			return CellObstacles[index];
		}
	}

	public struct FogOfWarCell
	{
		public bool Occluded;
		public CellVisibilityState State;
	}

	public struct FogOfWar : IComponentData
	{
		public bool Explored;
		public int2 Size;
		public NativeArray<bool> Occlusion;
		public NativeArray<CellVisibilityState> Visibility;
	}
}
