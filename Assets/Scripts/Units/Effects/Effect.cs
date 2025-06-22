using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public struct Effect : IBufferElementData
	{
		public UnityObjectRef<EffectDesc> Desc;

		public float Time;
	}
}
