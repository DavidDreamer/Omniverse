using Unity.Burst;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse.Abilities
{
	public struct BuildInput : IInputComponentData
	{
		public int BlueprintIndex;
		public LocalTransform LocalTransform;
		public int Faction;
		public InputEvent Event;
	}
}
