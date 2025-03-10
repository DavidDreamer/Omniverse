using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

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

		[UpdateInGroup(typeof(InputSystemGroup))]
		public partial struct DetectEntity : ISystem
		{
			public void OnUpdate(ref SystemState state)
			{
				var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
				
				Camera camera = Camera.main;

				if (camera == null)
				{
					return;
				}

				Mouse mouse = Mouse.current;
				Vector2 mousePosition = Mouse.current.position.value;
				UnityEngine.Ray ray = camera.ScreenPointToRay(mousePosition);

				RaycastInput input = new RaycastInput
				{
					Start = ray.origin,
					End = ray.origin + ray.direction * 10000f,
					Filter = new CollisionFilter()
					{
						BelongsTo = ~0u,
						CollidesWith= ~0u,
					}
				};

				if (physicsWorld.CastRay(input, out Unity.Physics.RaycastHit raycastHit))
				{
					Debug.Log(raycastHit.Entity.ToString());
				}
			}
		}
	}
}
