namespace Omniverse.Entities.Units
{
	public struct ChangePropertyData
	{
		public PropertyID ID { get; set; }

		public IEntity Source { get; set; }

		public float Amount { get; set; }
	}
}
