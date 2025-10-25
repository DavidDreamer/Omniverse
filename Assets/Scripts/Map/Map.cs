using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct Map : IComponentData
	{
		public int2 Size;
		public NativeArray<Node> Nodes;

		public Node NodeFromPosition(float3 position)
		{
			int x = (int)math.floor(position.x) + Size.x / 2;
			int y = (int)math.floor(position.z) + Size.y / 2;

			int id = y * Size.x + x;
			return Nodes[id];
		}
	}
}
