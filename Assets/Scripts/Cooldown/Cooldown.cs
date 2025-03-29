using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	public struct Cooldown : IComponentData
	{
		[GhostField]
		public float Duration;

		[GhostField]
		public float TimeLeft;
	}
}
