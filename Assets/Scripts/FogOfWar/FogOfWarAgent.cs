using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{

	public struct FogOfWarAgent : IComponentData
	{
		public float VisionRange;

		public int CellIndex;
	}
}
