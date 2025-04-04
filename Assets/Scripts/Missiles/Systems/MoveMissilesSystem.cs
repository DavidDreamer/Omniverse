using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
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
