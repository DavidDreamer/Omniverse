using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Abilities
{
	[BurstCompile]
	public struct Blueprint : IBufferElementData
	{
		public Entity Building;
	}
}
