using Unity.Entities;
using Unity.NetCode;

namespace Omniverse.Input
{
	public class InputSystemData : IComponentData
	{
		public InputActions InputActions;
	}

	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation, WorldSystemFilterFlags.ClientSimulation)]
	[UpdateInGroup(typeof(GhostInputSystemGroup))]
	public partial class InputSystemGroup : ComponentSystemGroup
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			//TODO ECS: It's strange, but that's the only way managed singletons work

			var entity = EntityManager.CreateEntity();
			EntityManager.AddComponent<InputSystemData>(entity);

			var inputSystemData = new InputSystemData()
			{
				InputActions = new InputActions()
			};

			inputSystemData.InputActions.Common.Enable();
			inputSystemData.InputActions.Abilities.Enable();

			EntityManager.SetComponentData(entity, inputSystemData);
		}
	}
}
