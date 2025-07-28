using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	public struct UnitInput : IInputComponentData
	{
		[GhostField]
		public Command Command;

		[GhostField]
		public float3 Position;

		[GhostField]
		public Entity Entity;

		public InputEvent Event;
	}
}
