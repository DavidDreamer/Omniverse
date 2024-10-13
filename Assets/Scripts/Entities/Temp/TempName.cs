namespace Omniverse
{
	public abstract class TempName : Entity
	{
		public bool Completed { get; protected set; }

		public abstract void Tick(float deltaTime);
	}
}
