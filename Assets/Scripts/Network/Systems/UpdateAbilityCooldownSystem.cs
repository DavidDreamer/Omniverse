using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse.Network
{
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach (var cooldown in SystemAPI.Query<RefRW<Cooldown>>())
			{
				cooldown.ValueRW.TimeLeft = math.max(0f, cooldown.ValueRW.TimeLeft - deltaTime);
			}
		}
	}
}
