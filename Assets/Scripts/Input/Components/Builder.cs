using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct Builder : IComponentData
	{
		public Entity Building;

		public bool InProcess => Building != Entity.Null;
	}
}
