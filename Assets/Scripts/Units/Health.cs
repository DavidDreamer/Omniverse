using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	//[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	//public partial struct HealthSustem : ISystem
	//{
	//	public void OnUpdate(ref SystemState state)
	//	{
	//		foreach (var health in SystemAPI.Query<RefRW<Health>>())
	//		{
	//			health.ValueRW.Current -= 0.01f;
	//		}
	//	}
	//}

	[BurstCompile]
	public struct Health : IComponentData
	{
		[GhostField]
		public float Maximum;

		[GhostField]
		public float Current;
	}
}
