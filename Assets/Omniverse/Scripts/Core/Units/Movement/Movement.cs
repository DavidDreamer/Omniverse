namespace Omniverse
{
	public class Movement
	{
		public MovementDesc Desc { get; }

		public float Speed { get; private set; }
		
		public float RotationSpeed { get; private set; }

		public Movement(MovementDesc desc)
		{
			Desc = desc;

			Speed = Desc.Speed;
			RotationSpeed = Desc.RotationSpeed;
		}
	}
}
