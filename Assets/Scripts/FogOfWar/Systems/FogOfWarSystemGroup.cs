using Unity.Collections;
using Unity.Entities;

namespace Omniverse
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public partial class FogOfWarSystemGroup : ComponentSystemGroup
	{
		protected override void OnStartRunning()
		{
			base.OnStartRunning();

			var settings = SystemAPI.GetSingleton<FogOfWarSettings>();

			if (settings.Mode is FogOfWarMode.Revealed)
			{
				return;
			}

			var entity = EntityManager.CreateEntity();
			EntityManager.SetName(entity, "Fog Of War");
			EntityManager.AddComponent<FogOfWar>(entity);
			EntityManager.SetComponentData(entity, new FogOfWar()
			{
				Occlusion = new NativeArray<bool>(settings.Size.x * settings.Size.y, Allocator.Persistent),
				Visibility = new NativeArray<CellVisibilityState>(settings.Size.x * settings.Size.y, Allocator.Persistent)
			});
		}

		protected override void OnStopRunning()
		{
			base.OnStopRunning();

			if (SystemAPI.HasSingleton<FogOfWar>())
			{
				var fogOfWar = SystemAPI.GetSingleton<FogOfWar>();
				fogOfWar.Occlusion.Dispose();
				fogOfWar.Visibility.Dispose();
			}
		}
	}
}
