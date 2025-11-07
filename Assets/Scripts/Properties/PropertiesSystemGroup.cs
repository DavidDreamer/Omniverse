using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
	public partial class PropertiesSystemGroup : ComponentSystemGroup
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			EnableSystemSorting = false;

			CreateSystem<ResetPropertiesSystem>();
			CreateSystem<ApplyAreaModifiersSystem>();
			CreateSystem<ApplyEffectsSystem>();
			CreateSystem<CalculatePropertiesTotalSystem>();

			void CreateSystem<T>() where T : unmanaged, ISystem
			{
				SystemHandle systemHandle = World.CreateSystem<T>();
				AddSystemToUpdateList(systemHandle);
			}
		}
	}
}
