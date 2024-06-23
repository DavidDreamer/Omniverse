namespace Omniverse.Entities.Units
{
	public static class PropertyUtils
	{
		public static bool IsFull(this Property property) => property.Amount.Value == property.Desc.Range.Max;
	}
}
