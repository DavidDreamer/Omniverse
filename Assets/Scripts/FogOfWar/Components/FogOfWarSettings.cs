using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct FogOfWarSettings : IComponentData
	{
		public FogOfWarMode Mode;
		public int2 Size;
	}
}
