using Unity.Entities;

namespace Omniverse.Input
{
	public struct AbilityInput : IComponentData
	{
		public Entity Ability;

		public bool InProcess => Ability != Entity.Null;
	}
}
