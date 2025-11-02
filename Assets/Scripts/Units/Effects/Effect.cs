using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[GhostComponent]
	public struct Effect : IBufferElementData
	{
		[GhostField(SendData = false)]
		public UnityObjectRef<EffectDesc> Desc;

		[GhostField]
		public float Time;
	}
}
