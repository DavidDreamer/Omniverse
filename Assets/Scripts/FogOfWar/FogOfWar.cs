using Unity.Collections;
using Unity.Entities;

namespace Omniverse
{
	public struct FogOfWar : IComponentData
	{
		public NativeArray<bool> Occlusion;
		public NativeArray<CellVisibilityState> Visibility;
	}
}
