namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Entity Entity { get; }

		public Casting Casting { get; }

		public Cooldown Cooldown { get; }

		public Ability(AbilityDesc desc, Entity entity)
		{
			Desc = desc;
			Entity = entity;

			Casting = new Casting(Desc.Casting);
			Cooldown = new Cooldown(Desc.Cooldown);

			for (int i = 0; i < Desc.Triggers.Length; i++)
			{
				IAbilityTrigger trigger = Desc.Triggers[i];
				trigger.Listen(entity);
			}
		}

		public void Tick(float deltaTime)
		{
			Cooldown.Tick(deltaTime);
		}

		public void Cast<TTarget>(TTarget target)
		{
			Cooldown.Activate();

			var operation = (IOperation<TTarget>)Desc.ActiveOperation;
			operation.Perform(Entity, target);
		}
	}
}
