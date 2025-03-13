namespace Omniverse
{
	public abstract class TempName : OmniverseEntity
	{
		public bool Completed { get; protected set; }

		public abstract void Tick(float deltaTime);
	}
}
