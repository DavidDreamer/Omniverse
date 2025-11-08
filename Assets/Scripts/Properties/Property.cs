using Unity.Burst;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[GhostComponent]
	public struct Property
	{
		[GhostField]
		public float Base;

		[GhostField]
		public float Additional;

		[GhostField]
		public float Multipler;

		[GhostField]
		public float Total;

		public void Reset()
		{
			Additional = 0;
			Multipler = 1;
		}

		public void CalculateTotal()
		{
			Total = (Base + Additional) * Multipler;
		}
	}
}
