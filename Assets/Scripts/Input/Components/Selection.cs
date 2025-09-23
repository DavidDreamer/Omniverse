using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse.Input
{
	[BurstCompile]
	public struct Selection : IComponentData
	{
		public const int Capacity = 16;
		public const float SelectionBoxTreshold = 16;

		public NativeList<Entity> Entities;

		public bool HasSelection => Entities.Length > 0;

		public Entity Entity => Entities[SelectionIndex];

		public int SelectionIndex;

		public float2 StartPosition;

		public float2 EndPosition;

		public bool InProcess;

		public int AbilityIndex;

		public bool AbilityInProcess;
	}
}
