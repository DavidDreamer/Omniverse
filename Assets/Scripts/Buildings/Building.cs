using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public struct Building : IComponentData
	{
		public UnityObjectRef<BuildingDesc> Desc;
	}
}
