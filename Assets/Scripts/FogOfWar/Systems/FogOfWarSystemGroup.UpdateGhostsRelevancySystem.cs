using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	public partial class FogOfWarSystemGroup
	{
		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateVisibilitySystem))]
		[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
		public partial struct UpdateGhostsRelevancySystem : ISystem
		{
			[BurstCompile]
			public void OnCreate(ref SystemState state)
			{
				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

				if (fogOfWarSettings.Mode is FogOfWarMode.Revealed)
				{
					return;
				}

				var ghostRelevancy = SystemAPI.GetSingletonRW<GhostRelevancy>();
				ghostRelevancy.ValueRW.GhostRelevancyMode = GhostRelevancyMode.SetIsRelevant;
			}

			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

				if (fogOfWarSettings.Mode is FogOfWarMode.Revealed)
				{
					return;
				}

				var ghostRelevancy = SystemAPI.GetSingletonRW<GhostRelevancy>();
				ghostRelevancy.ValueRW.GhostRelevancySet.Clear();

				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;
			
				foreach ((var localTransform, var ghostInstance) in SystemAPI.Query<LocalTransform, GhostInstance>().WithAll<Unit>().WithAll<Simulate>())
				{
					int cellIndex = FogOfWarUtils.CellIndexFromPosition(localTransform.Position, mapSize, fogOfWarSettings.Size);

					foreach (var fogOfWar in SystemAPI.Query<FogOfWar>())
					{
						bool isVisible = fogOfWar.Visibility[cellIndex] is CellVisibilityState.Visible;

						if (isVisible)
						{
							var a = new RelevantGhostForConnection()
							{
								Connection = 1,
								Ghost = ghostInstance.ghostId
							};

							ghostRelevancy.ValueRW.GhostRelevancySet.Add(a, ghostInstance.ghostId);
						}
					}
				}
			}
		}
	}
}
