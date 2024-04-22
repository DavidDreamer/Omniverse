namespace Omniverse
{
	public class AttackModule
	{
		private AttackDesc Desc { get; }
		
		public float Range { get; private set; }
		
		public float Speed { get; private set; }
		
		public float Damage { get; private set; }

		public AttackModule(AttackDesc desc)
		{
			Desc = desc;

			Range = Desc.Range;
			Speed = Desc.Speed;
			Damage = Desc.Damage;
		}
		
		public void Perform(Unit target)
		{
			
		}
	}
}
