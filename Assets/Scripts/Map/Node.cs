using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct Node
	{
		public int Id;
		public int2 Coordinates;
		public NativeArray<NeighbourNodeData> Neighbours;
	}
}
