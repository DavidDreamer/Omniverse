namespace Omniverse
{
	public partial class MissileObsolete
	{
		private abstract class Behaviour
		{
			protected MissileObsolete Missile { get; }

			public Behaviour(MissileObsolete missile)
			{
				Missile = missile;
			}

			public abstract void Tick(float deltaTime);
		}
	}
}
