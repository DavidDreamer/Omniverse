using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Movement : IComponentData
	{
		[GhostField]
		public Property Speed;

		[GhostField]
		public Property TurnRate;
	}
}
