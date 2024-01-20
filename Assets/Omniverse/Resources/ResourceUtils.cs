namespace Omniverse
{
	public static class ResourceUtils
	{
		public static bool IsFull(this Resource resource) => resource.Amount.Value == resource.Capacity.Value;
	}
}
