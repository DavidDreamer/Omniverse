using System.Numerics;
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

				if (networkTime.IsFirstTimeFullyPredictingTick)
				{
					if (unitInput.ValueRW.Event.IsSet)
					{
						switch (unitInput.ValueRW.Command)
						{
							case Command.Idle:
								break;
							case Command.Move:
								waypoints.Clear();

								Node start = map.NodeFromPosition(localTransform.ValueRW.Position);
								Node goal = map.NodeFromPosition(unitInput.ValueRW.Position);
								var nodes = AStar.FindPath(map, start, goal);

								if (nodes != null)
								{
									foreach (var node in nodes)
									{
										Waypoint waypoint = new()
										{
											Position = new float3(node.Coordinates.x + 0.5f, 0, node.Coordinates.y + 0.5f)
										};

										waypoints.Add(waypoint);
									}
								}
						
								break;
						}
					}
				}

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
