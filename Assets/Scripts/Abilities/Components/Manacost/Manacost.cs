using Unity.NetCode;

namespace Omniverse
{
	public struct Manacost
	{
		[GhostField]
		public float Value;

		[GhostField]
		public PropertyModifierMode Mode;
	}
}
