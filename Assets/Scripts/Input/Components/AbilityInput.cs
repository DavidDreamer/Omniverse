using Unity.Entities;

namespace Omniverse.Input
{
	public class AbilityInput : IComponentData
	{
		public Entity Ability;

		public bool InProcess => Ability != Entity.Null;
	}
}
