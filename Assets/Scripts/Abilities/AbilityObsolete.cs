namespace Omniverse.Abilities
{
	public class AbilityObsolete
	{
		public AbilityDesc Desc { get; }

		public OmniverseEntity Entity { get; }

		public AbilityObsolete(AbilityDesc desc, OmniverseEntity entity)
		{
			Desc = desc;
			Entity = entity;

			for (int i = 0; i < Desc.Triggers.Length; i++)
			{
				IAbilityTrigger trigger = Desc.Triggers[i];
				trigger.Listen(entity);
			}
		}

		public void Cast<TTarget>(TTarget target)
		{
			var operation = (IOperation<TTarget>)Desc.ActiveOperation;
			operation.Perform(Entity, target);
		}
	}
}
