namespace Omniverse.Entities.Units
{
	public struct ChangePropertyData
	{
		public PropertyID ID { get; set; }

		public Entity Source { get; set; }

		public float Amount { get; set; }
	}
}
