using Unity.Entities;

namespace Omniverse.Input
{
	public class InputSystemData : IComponentData
	{
		public InputActions InputActions;
	}

	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
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

			CreateManagedSingleton<AbilityInput>();

			void CreateManagedSingleton<T>() where T : class, new()
			{
				var entity = EntityManager.CreateEntity();
				EntityManager.AddComponent<T>(entity);
				var component = new AbilityInput();
				EntityManager.SetComponentData(entity, component);
			}
		}
	}
}
