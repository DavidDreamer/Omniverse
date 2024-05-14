namespace Omniverse.Units
{
	public static class PropertyUtils
	{
		public static bool IsFull(this Property property) => property.Amount.Value == property.Capacity.Value;
	}
}
