using Unity.Entities;

namespace Omniverse
{
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	[DisableAutoCreation]
	public partial struct DecreaseHealthSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (var health in SystemAPI.Query<RefRW<Health>>())
			{
				health.ValueRW.Current -= 0.01f;
			}
		}
	}
}
