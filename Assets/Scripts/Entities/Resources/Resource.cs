using Cysharp.Threading.Tasks;

namespace Omniverse
{
	public class Resource
	{
		public ResourceDesc Desc { get; }

		public AsyncReactiveProperty<int> Amount { get;  }
		
		public Resource(ResourceDesc desc)
		{
			Desc = desc;

			Amount = new AsyncReactiveProperty<int>(0);
		}
	}
}
