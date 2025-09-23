using Unity.NetCode;

namespace Omniverse
{
	public struct Cooldown
	{
		[GhostField]
		public float Duration;

		[GhostField]
		public float TimeLeft;

		public bool Active => TimeLeft > 0f;
	}
}
