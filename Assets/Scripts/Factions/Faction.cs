using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public struct Faction : ISharedComponentData
	{
		public int ID;
	}
}
