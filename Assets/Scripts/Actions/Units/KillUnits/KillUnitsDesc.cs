using System;

namespace Omniverse.Actions
{
	[Serializable]
	public class KillUnitsDesc : IActionDesc
	{
		public IAction Build() => new KillUnits(this);
	}
}
