using Omniverse.Abilities;
using Unity.Entities;
using Unity.NetCode;

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

	public struct Owner : IComponentData
	{
		[GhostField]
		public Entity Entity;
	}

	public struct AbilityReference : IBufferElementData
	{
		[GhostField]
		public Entity Entity;
	}

	public struct Ability : IComponentData
	{
	}
}
