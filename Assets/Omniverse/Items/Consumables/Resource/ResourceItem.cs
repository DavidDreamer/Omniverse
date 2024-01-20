namespace Omniverse
{
	public class ResourceItem: ConsumableItem<ResourceItemDesc>
	{
		public ResourceItem(ResourceItemDesc desc): base(desc)
		{
		}

		public override bool CanBeConsumed(Unit unit)
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

		public override void OnConsumed(Unit unit)
		{
			Resource resource = unit.Resources[Desc.ResourceID];
			resource.Change(Desc.Amount);
		}
	}
}
