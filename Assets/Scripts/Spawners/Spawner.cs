using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct Spawner : IComponentData
	{
		public Entity Prefab;
		public float3 Position;
		public float Interval;
		public float NextSpawnTime;
		public float Speed;
	}
}
