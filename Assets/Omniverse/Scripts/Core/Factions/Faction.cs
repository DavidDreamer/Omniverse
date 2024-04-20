using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Omniverse
{
	public class Faction
	{
		public int ID { get; }
		
		private FactionDesc Desc { get; }
		
		public Dictionary<ResourceDesc, AsyncReactiveProperty<int>> Resources { get; } = new();

		public Faction(int id, FactionDesc desc)
		{
			ID = id;
			Desc = desc;
		}

		public void ChangeCurrency(ResourceDesc resource, int amount)
		{
			if (Resources.ContainsKey(resource) is false)
			{
				Resources.Add(resource, new AsyncReactiveProperty<int>(0));
			}

			Resources[resource].Value += amount;
		}
	}
}
