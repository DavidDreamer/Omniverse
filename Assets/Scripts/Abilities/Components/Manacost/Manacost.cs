using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Manacost : IComponentData
	{
		[GhostField]
		public float Value;

		[GhostField]
		public PropertyModifierMode Mode;
	}
}
