using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Missile : IComponentData
	{
		[GhostField]
		public float Speed;

		[GhostField]
		public float3 Direction;

		[GhostField]
		public float3 StartPosition;
	}
}
