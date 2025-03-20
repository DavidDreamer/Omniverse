using Unity.Entities;

namespace Omniverse.Input
{
	public class AbilityInput : IComponentData
	{
		public Entity Entity;

		public Ability Ability;

		public bool InProcess;
	}
}
