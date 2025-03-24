using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct Faction : IComponentData
	{
		[GhostField]
		public int ID;
	}
}
