using Unity.Entities;

namespace Omniverse
{
	public struct FogOfWarAgent : IComponentData
	{
		public float VisionRange;

		public int CellIndex;
	}
}
