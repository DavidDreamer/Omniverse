using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct Obstacle : IComponentData
	{
		public int2 Start;
		public int2 End;
	}
}
