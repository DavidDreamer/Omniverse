using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Abilities
{
	[BurstCompile]
	public struct BuildAbility : IComponentData
	{
		public Entity Building;
	}
}
