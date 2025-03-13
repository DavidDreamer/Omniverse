using System;

namespace Omniverse
{
	[Flags]
	public enum UnitStatus
	{
		Stunned = 0x1,
		Rooted = 0x2,
		Silenced = 0x4,
		Invisible = 0x8
	}
}
