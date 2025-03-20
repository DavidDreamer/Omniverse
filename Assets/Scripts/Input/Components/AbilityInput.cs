using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct AbilityInput : IComponentData
	{
		public Entity Entity;

		public int AbilityIndex;

		public bool InProcess;
	}
}
