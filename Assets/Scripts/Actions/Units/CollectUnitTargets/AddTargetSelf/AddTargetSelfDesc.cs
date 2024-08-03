using System;

namespace Omniverse.Actions
{
	[Serializable]
	public class AddTargetSelfDesc: IActionDesc
	{
		public IAction Build() => new AddTargetSelf(this);
	}
}
