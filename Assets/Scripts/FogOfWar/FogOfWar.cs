using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct FogOfWar : IComponentData
	{
		public bool Explored;
		public int2 Size;
		public NativeArray<bool> Occlusion;
		public NativeArray<CellVisibilityState> Visibility;
	}
}
