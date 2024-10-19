using System.Collections.Generic;

namespace Omniverse
{
	public class Faction
	{
		public int ID { get; }

		private FactionDesc Desc { get; }

		public Dictionary<ResourceDesc, Resource> Resources { get; } = new();

		public Faction(int id, FactionDesc desc, ResourceDesc[] resourceDescs)
		{
			ID = id;
			Desc = desc;

			foreach (ResourceDesc resourceDesc in resourceDescs)
			{
				var resource = new Resource(resourceDesc);
				Resources.Add(resourceDesc, resource);
			}
		}

		public void ChangeResource(ResourceDesc desc, int amount)
		{
			Resources[desc].Amount += amount;
		}
	}
}
