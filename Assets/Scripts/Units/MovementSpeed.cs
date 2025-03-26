using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct MovementSpeed : IComponentData
	{
		[GhostField]
		public float Current;
	}
}
