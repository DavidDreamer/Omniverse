namespace Omniverse.Entities
{
	public static class PropertyUtils
	{
		public static bool IsFull(this Property property) => property.Amount.Value == property.Desc.Range.Max;
	}
}
