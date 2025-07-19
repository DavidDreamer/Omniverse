using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Range : IComponentData
	{
		[GhostField]
		public float Value;
	}
}
