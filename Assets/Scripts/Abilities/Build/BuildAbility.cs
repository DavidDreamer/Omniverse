using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Abilities
{
	[BurstCompile]
	public struct BuildAbility : IComponentData
	{
		public UnityObjectRef<BuildAbilityDesc> Desc;
		public Entity Building;
	}
}
