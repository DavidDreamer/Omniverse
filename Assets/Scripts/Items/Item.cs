using Omniverse.Abilities;
using VContainer;

namespace Omniverse.Items
{
	public class Item : Entity<ItemDesc>, IPoolObject
	{
		public Ability Ability { get; set; }

		[Inject]
		private ActionHandler OperationHandler { get; set; }

		public override void Initialize(ItemDesc desc)
		{
			base.Initialize(desc);

			if (desc.Ability is not null)
			{
				Ability = new Ability(desc.Ability, this, OperationHandler);
			}
		}

		public void Cleanup()
		{
		}
	}
}
