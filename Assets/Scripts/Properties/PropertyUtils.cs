namespace Omniverse
{
	public static class PropertyUtils
	{
		public static bool IsFull(this PropertyObsolete property) => property.Amount == property.Desc.Range.Max;
	}
}
