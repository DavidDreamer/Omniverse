namespace Omniverse
{
	public interface IEntity
	{
		int FactionID { get; }
	}

	public interface IEntity<out TDesc>: IEntity
	{
		TDesc Desc { get; }
	}
	
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
