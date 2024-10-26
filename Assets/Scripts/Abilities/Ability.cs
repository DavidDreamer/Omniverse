using System.Linq;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Entity Entity { get; }

		public Casting Casting { get; }

		public Cooldown Cooldown { get; }

		public Ability(AbilityDesc desc, Entity actor)
		{
			Desc = desc;
			Entity = actor;

			Casting = new Casting(Desc.Casting);
			Cooldown = new Cooldown(Desc.Cooldown);
		}

		public void Tick(float deltaTime)
		{
			Cooldown.Tick(deltaTime);
		}

		public void Cast<TTarget>(TTarget target)
		{
			Cooldown.Activate();

			foreach (IOperation<TTarget> operation in Desc.Operations.Cast<IOperation<TTarget>>())
			{
				operation.Perform(Entity, target);
			}
		}
	}
}
