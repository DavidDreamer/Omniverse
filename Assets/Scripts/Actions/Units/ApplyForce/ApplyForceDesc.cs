using System;

namespace Omniverse.Actions
{
	[Serializable]
	public class ApplyForceDesc : IActionDesc
	{
		public IAction Build() => new ApplyForce(this);
	}
}
