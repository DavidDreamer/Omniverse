using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.AI;

#pragma warning disable 612, 618
namespace Omniverse
{
	[BurstCompile]
	public partial struct NavAgentSystem : ISystem
	{
		[BurstCompile]
		private void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;
			
			foreach (var (navAgent, movementSpeed, transform, entity) in SystemAPI.Query<RefRW<NavAgentComponent>, RefRW<MovementSpeed>, RefRW<LocalTransform>>().WithEntityAccess())
			{
				DynamicBuffer<WaypointBuffer> waypointBuffer = state.EntityManager.GetBuffer<WaypointBuffer>(entity);

				CalculatePath();
				Move();

				void CalculatePath()
				{
					NavMeshQuery query = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.TempJob, 1000);

					float3 fromPosition = transform.ValueRO.Position;
					float3 toPosition = navAgent.ValueRO.targetPosition;
					float3 extents = new float3(1, 1, 1);

					NavMeshLocation fromLocation = query.MapLocation(fromPosition, extents, 0);
					NavMeshLocation toLocation = query.MapLocation(toPosition, extents, 0);

					PathQueryStatus status;
					PathQueryStatus returningStatus;
					int maxPathSize = 100;

					if (query.IsValid(fromLocation) && query.IsValid(toLocation))
					{
						status = query.BeginFindPath(fromLocation, toLocation);
						if (status == PathQueryStatus.InProgress)
						{
							status = query.UpdateFindPath(100, out int iterationsPerformed);
							if (status == PathQueryStatus.Success)
							{
								status = query.EndFindPath(out int pathSize);

								NativeArray<NavMeshLocation> result = new NativeArray<NavMeshLocation>(pathSize + 1, Allocator.Temp);
								NativeArray<StraightPathFlags> straightPathFlag = new NativeArray<StraightPathFlags>(maxPathSize, Allocator.Temp);
								NativeArray<float> vertexSide = new NativeArray<float>(maxPathSize, Allocator.Temp);
								NativeArray<PolygonId> polygonIds = new NativeArray<PolygonId>(pathSize + 1, Allocator.Temp);
								int straightPathCount = 0;

								query.GetPathResult(polygonIds);

								returningStatus = PathUtils.FindStraightPath
									(
									query,
									fromPosition,
									toPosition,
									polygonIds,
									pathSize,
									ref result,
									ref straightPathFlag,
									ref vertexSide,
									ref straightPathCount,
									maxPathSize
									);

								if (returningStatus == PathQueryStatus.Success)
								{
									waypointBuffer.Clear();

									foreach (NavMeshLocation location in result)
									{
										if (location.position != Vector3.zero)
										{
											waypointBuffer.Add(new WaypointBuffer { wayPoint = location.position });
										}
									}

									navAgent.ValueRW.currentWaypoint = 0;
								}
								straightPathFlag.Dispose();
								polygonIds.Dispose();
								vertexSide.Dispose();
							}
						}
					}
					query.Dispose();
				}

				void Move()
				{
					if (waypointBuffer.Length == 0)
					{
						return;
					}

					if (math.distance(transform.ValueRO.Position, waypointBuffer[navAgent.ValueRO.currentWaypoint].wayPoint) < 0.4f)
					{
						if (navAgent.ValueRO.currentWaypoint + 1 < waypointBuffer.Length)
						{
							navAgent.ValueRW.currentWaypoint += 1;
						}
					}

					float3 direciton = waypointBuffer[navAgent.ValueRO.currentWaypoint].wayPoint - transform.ValueRO.Position;
					float angle = math.degrees(math.atan2(direciton.z, direciton.x));

					transform.ValueRW.Rotation = math.slerp(
									transform.ValueRW.Rotation,
									quaternion.Euler(new float3(0, angle, 0)),
									deltaTime);

					transform.ValueRW.Position += math.normalize(direciton) * deltaTime * movementSpeed.ValueRO.Current;
				}
			}


		}
	}
}
#pragma warning restore 612, 618