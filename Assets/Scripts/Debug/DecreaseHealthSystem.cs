using Unity.Entities;
using Unity.Mathematics;

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
				health.ValueRW.Current = math.max(0f, health.ValueRW.Current - 1f);
			}
		}
	}
}
