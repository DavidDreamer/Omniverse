using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct SpawnPoint : IBufferElementData
	{
		public float3 Position;

		public quaternion Rotation;
	}
}
