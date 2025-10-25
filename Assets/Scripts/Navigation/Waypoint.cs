using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct Waypoint : IBufferElementData
	{
		public float3 Position;
	}
}
