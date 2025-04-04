using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct Missile : IComponentData
	{
		public float Speed;
		public float3 Direction;
	}
}
