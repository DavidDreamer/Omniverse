using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial struct ProcessCommandsSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<Map>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var networkTime = SystemAPI.GetSingleton<NetworkTime>();
			var map = SystemAPI.GetSingleton<Map>();

			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((var unitInput, var entity) in SystemAPI.Query<RefRW<UnitInput>>().WithAll<Unit>().WithAll<Simulate>().WithEntityAccess())
			{
				var localTransform = SystemAPI.GetComponentRW<LocalTransform>(entity);
				var movementSpeed = SystemAPI.GetComponent<Movement>(entity);
				var waypoints = SystemAPI.GetBuffer<Waypoint>(entity);

				switch (unitInput.ValueRW.Command)
				{
					case Command.Idle:
						break;
					case Command.Move:
					{
						if (waypoints.Length > 0)
						{
							float3 goal = waypoints[0].Position;
							float3 vector = goal - localTransform.ValueRW.Position;
							float3 direction = math.normalizesafe(vector);
							direction.y = 0;

							localTransform.ValueRW.Rotation = quaternion.LookRotation(direction, new float3(0f, 1f, 0f));

							float lenght = math.length(vector);
							float distance = math.min(movementSpeed.Speed.Total * deltaTime, lenght);
							float3 deltaPosition = direction * distance;

							localTransform.ValueRW.Position += deltaPosition;

							float3 from = localTransform.ValueRW.Position;
							for (int i = 0; i < waypoints.Length; i++)
							{
								var to = waypoints[i].Position;
								UnityEngine.Debug.DrawLine(from, to);
								from = to;
							}

							if (lenght < 0.1f)
							{
								waypoints.RemoveAt(0);
							}
						}
						else
						{
							unitInput.ValueRW.Command = Command.Idle;
						}

						break;
					}
					default: throw new System.Exception();
				}
			}
		}
	}
}
