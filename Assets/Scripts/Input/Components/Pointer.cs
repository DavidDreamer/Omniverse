using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse.Input
{
	public enum PointerTargetType
	{
		None,
		World,
		Entity,
		UI
	}

	[BurstCompile]
	public struct Pointer : IComponentData
	{
		public PointerTargetType TargetType;

		public Entity Entity;

		public float3 WorldPosition;

		public float3 CellPosiiton;

		private NativeHashSet<ComponentType> DetectableTypes;

		private FactiousFilter Filter;
	}
}
