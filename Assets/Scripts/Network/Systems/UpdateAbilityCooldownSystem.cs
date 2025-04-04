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

			foreach ((var cooldown, var entity) in SystemAPI.Query<RefRW<Cooldown>>().WithAll<Cooldown>().WithEntityAccess())
			{
				float timeLeft = math.max(0f, cooldown.ValueRW.TimeLeft - deltaTime);
				cooldown.ValueRW.TimeLeft = timeLeft;

				if (timeLeft == 0)
				{
					SystemAPI.SetComponentEnabled<Cooldown>(entity, false);
				}
			}
		}
	}
}
