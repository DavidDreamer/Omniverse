using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct AbilityInput : IComponentData
	{
		public Entity Entity;

		public Entity Ability;

		public bool InProcess;
	}
}
