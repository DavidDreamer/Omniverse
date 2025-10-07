using Omniverse.Abilities;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[GhostComponent]
	public struct Ability : IBufferElementData
	{
		[GhostField(SendData = false)]
		public UnityObjectRef<AbilityDesc> Desc;

		[GhostField]
		public Manacost Manacost;

		[GhostField]
		public Cooldown Cooldown;

		[GhostField]
		public Casting Casting;
	}
}
