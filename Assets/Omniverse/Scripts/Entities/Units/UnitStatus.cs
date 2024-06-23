using System;

namespace Omniverse.Entities.Units
{
	[Flags]
	public enum UnitStatus
	{
		None = 0,
		Stunned = 0x1,
		Rooted = 0x2
	}
}
