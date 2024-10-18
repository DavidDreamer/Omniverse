namespace Omniverse
{
	public partial class Missile
	{
		private abstract class Behaviour
		{
			protected Missile Missile { get; }

			public Behaviour(Missile missile)
			{
				Missile = missile;
			}

			public abstract void Tick(float deltaTime);
		}
	}
}
