using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

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
				Entity other = default;

				if (entityManager.HasComponent<Missile>(triggerEvent.EntityA))
				{
					missile = triggerEvent.EntityA;
					other = triggerEvent.EntityB;
				}
				else if (entityManager.HasComponent<Missile>(triggerEvent.EntityB))
				{
					other = triggerEvent.EntityA;
					missile = triggerEvent.EntityB;
				}
				else
				{
					continue;
				}

				var onDestroyTrigger = entityManager.GetComponentObject<OnDestroyTrigger>(missile);

				if (onDestroyTrigger != null)
				{
					Vector3 position = entityManager.GetComponentData<LocalTransform>(missile).Position;
					Object.Instantiate(onDestroyTrigger.Prefab, position, Quaternion.identity);
				}

				state.EntityManager.DestroyEntity(missile);

				//TODO DEAL DAMAGE
				if (entityManager.HasComponent<Invulnerable>(other))
				{
					continue;
				}

				if (entityManager.HasComponent<Health>(other))
				{
					var health = state.EntityManager.GetComponentData<Health>(other);
					health.Current -= 10;
					state.EntityManager.SetComponentData(other, health);
				}
			}
		}
	}
}
