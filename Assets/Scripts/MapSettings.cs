using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	[BurstCompile]
	public struct MapSettings : IComponentData
	{
		public int2 Size;
		public FogOfWarMode FogOfWarMode;
	}
}
