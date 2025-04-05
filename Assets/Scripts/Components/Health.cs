using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Health : IComponentData
	{
		[GhostField]
		public float Maximum;

		[GhostField]
		public float Current;
	}
}
