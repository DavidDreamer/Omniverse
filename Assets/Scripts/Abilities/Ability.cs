using Cysharp.Threading.Tasks;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Entity Entity { get; }

		public Cooldown Cooldown { get; }

		public bool InProcess { get; set; }

		public float CastTime { get; set; }

		public AutoResetUniTaskCompletionSource Used { get; }

		public OperationHandler OperationHandler { get; }

		public OperationContext OperationContext { get; }

		public Ability(AbilityDesc desc, Entity actor, OperationHandler operationHandler)
		{
			Desc = desc;
			Entity = actor;
			Cooldown = new Cooldown(Desc.Cooldown);
			Used = AutoResetUniTaskCompletionSource.Create();

			OperationHandler = operationHandler;

			OperationContext = new OperationContext(actor);
		}

		public void Tick(float deltaTime)
		{
			Cooldown.Tick(deltaTime);
		}

		public void Cast()
		{
			Cooldown.Activate();

			OperationHandler.PerformAsync(Desc.Operation, Entity, OperationContext, default).Forget();

			Used.TrySetResult();

			OperationContext.Clear();
		}
	}
}
