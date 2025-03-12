using Unity.Entities;

namespace Omniverse.Input
{
	public class InputSystemData : IComponentData
	{
		public InputActions InputActions;
	}

	public partial class InputSystemGroup : ComponentSystemGroup
	{
		protected override void OnCreate()
		{
			base.OnCreate();

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
