using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct WaypointBuffer : IBufferElementData
	{
		public float3 wayPoint;
	}
}
