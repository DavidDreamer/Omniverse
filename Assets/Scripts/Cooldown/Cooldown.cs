using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	public struct Cooldown : IComponentData
	{
		[GhostField]
		public float Time;

		[GhostField]
		public float TimeLeft;
	}
}
