using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct ProcessCommandsSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((var unitInput, var entity) in SystemAPI.Query<RefRW<UnitInput>>().WithAll<Unit>().WithAll<Simulate>().WithEntityAccess())
			{
				switch (unitInput.ValueRW.Command)
				{
					case Command.Idle:
						break;
					case Command.Move:
					{
						var localTransform = SystemAPI.GetComponentRW<LocalTransform>(entity);
						var movementSpeed = SystemAPI.GetComponent<Movement>(entity);

						float3 vector = unitInput.ValueRW.Position - localTransform.ValueRW.Position;
						float3 direction = math.normalize(vector);
						direction.y = 0;

						float lenght = math.length(vector);
						float distance = math.min(movementSpeed.Speed.Total * deltaTime, lenght);
						float3 deltaPosition = direction * distance;

						localTransform.ValueRW.Position += deltaPosition;

						if (lenght < 0.1f)
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
