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
}
