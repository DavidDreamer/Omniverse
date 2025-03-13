using Unity.Entities;

namespace Omniverse
{
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

	public struct Health : IComponentData
	{
		public float Maximum;

		public float Current;
	}
}
