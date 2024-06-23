using System;

namespace Omniverse.Entities
{
	[Flags]
	public enum EntityTargetType
	{
		Self = 0x1,
		Ally = 0x2,
		Enemy = 0x4
	}
}
