using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public struct Range : IComponentData
	{
		public float Value;
	}
}
