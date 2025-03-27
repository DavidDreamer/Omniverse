using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	public struct CastRange : IComponentData
	{
		public float Value;
	}

	public class AbilityTarget : IComponentData
	{
		public ITarget Target;
	}

	public struct Ability : IComponentData
	{
	}
}
