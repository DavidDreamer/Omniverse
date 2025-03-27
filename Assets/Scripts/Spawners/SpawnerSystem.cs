using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[BurstCompile]
	public partial struct SpawnerSystem : ISystem
	{
		public float Interval { get; set; }

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
			{
				ProcessSpawner(ref state, spawner);
			}
		}

		private void ProcessSpawner(ref SystemState state, RefRW<Spawner> spawner)
		{
			if (spawner.ValueRO.NextSpawnTime > SystemAPI.Time.ElapsedTime)
			{
				return;
			}

			int count = 30;

			for (int i = 0; i < count; ++i)
			{
				float angle = i * (360f / 30) * Mathf.Deg2Rad;
				float3 direction = new float3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
				Unity.Entities.Entity entity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
				state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(spawner.ValueRO.Position).Translate(new float3(0, 1, 0)));
				//state.EntityManager.AddComponent<MissileComponent>(entity);
				state.EntityManager.SetComponentData(entity, new Missile
				{
					Speed = spawner.ValueRO.Speed,
					Direction = direction
				});
			}

			spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.Interval;
		}
	}

	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	//[UpdateAfter(typeof(SpawnerSystem))]
	public partial struct MoveMissilesSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var moveJob = new MoveJob()
			{
				DeltaTime = SystemAPI.Time.fixedDeltaTime
			};

			moveJob.ScheduleParallel();
		}

		public partial struct MoveJob : IJobEntity
		{
			public float DeltaTime;

			public void Execute(ref Missile missile, ref LocalTransform transform)
			{
				transform = transform.Translate(missile.Direction * missile.Speed * DeltaTime);
			}
		}
	}
}
