namespace Omniverse
{
	public abstract class Entity<TDesc>: IEntity<TDesc> where TDesc: EntityDesc
	{
		public TDesc Desc { get; }

		public int FactionID { get; }
		
		protected Entity(TDesc desc, int factionID)
		{
			Desc = desc;
			FactionID = factionID;
		}
	}
}
