using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct Cooldown : IComponentData
	{
		public float Time;

		public float TimeLeft;

		public bool IsActive => TimeLeft > 0f;

		public float Ratio => math.saturate(TimeLeft / Time);
	}
}
