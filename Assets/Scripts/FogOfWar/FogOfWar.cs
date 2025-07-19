using Unity.Collections;
using Unity.Entities;

namespace Omniverse
{
	public struct FogOfWar : IComponentData
	{
		public const int Multiplier = 2;

		public NativeArray<bool> Occlusion;
		public NativeArray<CellVisibilityState> Visibility;
	}
}
