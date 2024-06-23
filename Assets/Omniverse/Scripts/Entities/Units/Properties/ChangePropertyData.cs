namespace Omniverse.Entities.Units
{
	public struct ChangePropertyData
	{
		public PropertyID ID { get; set; }

		public Unit Source { get; set; }

		public float Amount { get; set; }
	}
}
