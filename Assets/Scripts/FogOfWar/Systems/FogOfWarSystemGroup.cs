using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public partial class FogOfWarSystemGroup : ComponentSystemGroup
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			EnableSystemSorting = false;

			CreateSystem<RefreshSystem>();
			CreateSystem<UpdateObstaclesSystem>();
			CreateSystem<UpdateAgentPositionSystem>();
			CreateSystem<UpdateVisibilitySystem>();

			if (World.IsServer())
			{
				CreateSystem<UpdateGhostsRelevancySystem>();
			}

			void CreateSystem<T>() where T : unmanaged, ISystem
			{
				SystemHandle systemHandle = World.CreateSystem<T>();
				AddSystemToUpdateList(systemHandle);
			}
		}

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
