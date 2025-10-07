using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse.Network
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((var abilityBuffer, var entity) in SystemAPI.Query<DynamicBuffer<Ability>>().WithEntityAccess().WithAll<Simulate>())
			{
				for (int i = 0; i < abilityBuffer.Length; i++)
				{
					Ability ability = abilityBuffer.ElementAt(i);
					abilityBuffer.ElementAt(i).Cooldown.TimeLeft = math.max(0f, ability.Cooldown.TimeLeft - deltaTime);
				}
			}
		}
	}
}
