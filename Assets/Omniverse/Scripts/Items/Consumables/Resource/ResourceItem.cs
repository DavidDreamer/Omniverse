namespace Omniverse
{
	public class ResourceItem: Item<ResourceItemDesc>, IConsumableItem
	{
		public ResourceItem(ResourceItemDesc desc): base(desc)
		{
		}

		public bool CanBeConsumed(Unit unit)
		{
			if (unit.Resources.TryGetValue(Desc.ResourceID, out Resource resource) is false)
			{
				return false;
			}

			if (resource.IsFull())
			{
				return false;
			}

			return true;
		}

		public void OnConsumed(Unit unit)
		{
			Resource resource = unit.Resources[Desc.ResourceID];
			resource.Change(Desc.Amount);
		}
	}
}
