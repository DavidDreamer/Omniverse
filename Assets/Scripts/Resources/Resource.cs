namespace Omniverse
{
	public class Resource
	{
		public ResourceDesc Desc { get; }

		public int Amount { get; set; }

		public Resource(ResourceDesc desc)
		{
			Desc = desc;
		}
	}
}
