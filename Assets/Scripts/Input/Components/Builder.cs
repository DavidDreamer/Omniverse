using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct Builder : IComponentData
	{
		public UnityObjectRef<BuildingDesc> BuildingDesc;

		public bool InProcess => BuildingDesc != null;
	}
}
