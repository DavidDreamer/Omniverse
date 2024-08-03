using System;

namespace Omniverse.Input
{
	public class ErrorHandler
	{
		public Action<string> OnErrorHandle;

		public void Hadle(string error)
		{
			OnErrorHandle?.Invoke(error);
		}
	}
}
