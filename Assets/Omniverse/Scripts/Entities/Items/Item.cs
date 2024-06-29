using Omniverse.Abilities;

namespace Omniverse.Entities.Items
{
	public class Item: Entity<ItemDesc>, IPoolObject
	{
		public Ability Ability { get; set; }

		public override void Initialize(ItemDesc desc, int factionID)
		{
			base.Initialize(desc, factionID);
			
			if (desc.Ability is not null)
			{
				Ability = new Ability(desc.Ability);
			}
		}

		public void Cleanup()
		{
		}
	}
}
