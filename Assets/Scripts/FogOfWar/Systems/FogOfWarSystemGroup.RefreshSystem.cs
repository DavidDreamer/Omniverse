using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	public partial class FogOfWarSystemGroup
	{
		[BurstCompile]
		[DisableAutoCreation]
		public partial struct RefreshSystem : ISystem
		{
			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					if (fogOfWarSettings.Mode is FogOfWarMode.Explored)
					{
						for (int i = 0; i < fogOfWar.ValueRW.Visibility.Length; ++i)
						{
							fogOfWar.ValueRW.Visibility[i] = CellVisibilityState.Explored;
						}
					}
					else
					{
						for (int i = 0; i < fogOfWar.ValueRW.Visibility.Length; ++i)
						{
							if (fogOfWar.ValueRW.Visibility[i] is CellVisibilityState.Visible)
							{
								fogOfWar.ValueRW.Visibility[i] = CellVisibilityState.Explored;
							}
						}
					}
				}
			}
		}
	}
}
