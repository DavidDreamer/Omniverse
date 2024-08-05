using System;

namespace Omniverse.Abilities
{
	[Flags]
	public enum TargetType
	{
		None = 0,
		Point = 1 << 0,
		Direction = 1 << 1,
		Unit = 1 << 2,
		ResourceSource = 1 << 3
	}
}
