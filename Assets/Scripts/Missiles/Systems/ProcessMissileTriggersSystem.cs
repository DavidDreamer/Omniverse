using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PhysicsSystemGroup))]
	[UpdateAfter(typeof(PhysicsSimulationGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct ProcessMissileTriggersSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<SimulationSingleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			EntityManager entityManager = state.EntityManager;

			var simulation = SystemAPI.GetSingleton<SimulationSingleton>().AsSimulation();
	
			simulation.FinalSimulationJobHandle.Complete();
			foreach (TriggerEvent triggerEvent in simulation.TriggerEvents)
			{
				Entity entityA = triggerEvent.EntityA;
				Entity entityB = triggerEvent.EntityB;

				Entity missile = default;
				Entity health = default;
				bool isValid = false;

				if (entityManager.HasComponent<Missile>(entityA) && entityManager.HasComponent<Health>(entityB))
				{
					missile = entityA;
					health = entityB;
					isValid = true;
				}
				else if (entityManager.HasComponent<Health>(entityA) && entityManager.HasComponent<Missile>(entityB))
				{
					missile = entityB;
					health = entityA;
					isValid = true;
				}

				if (isValid)
				{
					state.EntityManager.DestroyEntity(missile);

					var h = state.EntityManager.GetComponentData<Health>(health);
					h.Current -= 10;
					state.EntityManager.SetComponentData(health, h);
				}
			}
		}
	}
}
