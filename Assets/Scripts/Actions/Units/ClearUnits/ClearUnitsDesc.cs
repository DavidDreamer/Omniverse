using System;

namespace Omniverse.Actions
{
	[Serializable]
	public class ClearUnitsDesc: IActionDesc
	{
		public IAction Build() => new ClearUnits(this);
	}
}
