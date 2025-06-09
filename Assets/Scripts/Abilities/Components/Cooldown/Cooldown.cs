using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[GhostEnabledBit]
	public struct Cooldown : IComponentData, IEnableableComponent
	{
		[GhostField]
		public float Duration;

		[GhostField]
		public float TimeLeft;
	}
}
