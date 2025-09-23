using Omniverse.Abilities;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Ability : IBufferElementData
	{
		public UnityObjectRef<AbilityDesc> Desc;
		public Manacost Manacost;
		public Cooldown Cooldown;
		public Casting Casting;
	}
}
