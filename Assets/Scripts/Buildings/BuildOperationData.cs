using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	public struct BuildOperationData
	{
		public Entity Building;
		public LocalTransform LocalTransform;
		public int Faction;
	}
}
