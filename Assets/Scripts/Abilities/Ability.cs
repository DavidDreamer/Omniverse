using Cysharp.Threading.Tasks;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Entity Entity { get; }

		public Casting Casting { get; }

		public Cooldown Cooldown { get; }

		public AutoResetUniTaskCompletionSource Used { get; }

		public ActionHandler ActionHandler { get; }

		public ActionContext ActionContext { get; }

		public Ability(AbilityDesc desc, Entity actor, ActionHandler actionHandler)
		{
			Desc = desc;
			Entity = actor;

			Casting = new Casting(Desc.Casting);
			Cooldown = new Cooldown(Desc.Cooldown);

			Used = AutoResetUniTaskCompletionSource.Create();

			ActionHandler = actionHandler;

			ActionContext = new ActionContext(actor);
		}

		public void Tick(float deltaTime)
		{
			Cooldown.Tick(deltaTime);
		}

		public void Cast()
		{
			Cooldown.Activate();

			ActionHandler.Perform(Desc.Action, ActionContext);

			Used.TrySetResult();

			ActionContext.Clear();
		}
	}
}
