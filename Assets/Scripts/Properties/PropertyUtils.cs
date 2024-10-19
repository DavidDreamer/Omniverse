namespace Omniverse
{
	public static class PropertyUtils
	{
		public static bool IsFull(this Property property) => property.Amount == property.Desc.Range.Max;
	}
}
