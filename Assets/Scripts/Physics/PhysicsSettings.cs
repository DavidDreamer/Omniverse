using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	[BurstCompile]
	public struct PhysicsSettings : IComponentData
	{
		public int HitboxLayer;

		public LayerMask HitboxLayerMask;
	}
}
