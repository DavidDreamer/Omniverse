using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse.Items
{
	public class Item : OmniverseEntity<ItemDesc>, IPoolObject
	{
		public Entity Ability { get; set; }

		public override void Initialize(ItemDesc desc)
		{
			base.Initialize(desc);

			//TODO ECS
			//if (desc.Ability is not null)
			//{
			//	Ability = new Ability(desc.Ability, this);
			//}
		}

		public void Cleanup()
		{
		}
	}
}
