using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessPointerSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.EntityManager.CreateSingleton<Pointer>();
		}

		public void OnUpdate(ref SystemState state)
		{
			//TODO ECS
			//if (abilityInProcess)
			//{
			//	entityDetector.SetupForAbility(AbilityController.ActiveAbility);
			//}
			//else
			//{
			//	entityDetector.SetDefaultDetectableType();
			//}

			//if (abilityInProcess)
			//{
			//	return;
			//}

			var pointer = SystemAPI.GetSingleton<Pointer>();

			bool pointerIsOverUI = EventSystem.current.IsPointerOverGameObject();

			if (pointerIsOverUI)
			{
				pointer.TargetType = PointerTargetType.UI;
			}
			else
			{
				var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
				var physicsSettings = SystemAPI.GetSingleton<PhysicsSettings>();

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
						CollidesWith = (uint)physicsSettings.HitboxLayerMask.value,
					}
				};

				if (physicsWorld.CastRay(input, out Unity.Physics.RaycastHit raycastHit))
				{
					pointer.TargetType = PointerTargetType.Entity;
					pointer.Entity = raycastHit.Entity;
				}
				else
				{
					if (NavmeshUtils.GetNavMeshPositionFromCursor(ray, out Vector3 position))
					{
						pointer.TargetType = PointerTargetType.World;
						pointer.WorldPosition = position;
					}
					else
					{
						pointer.TargetType = PointerTargetType.None;
					}
				}

				SystemAPI.SetSingleton(pointer);
			}
		}
	}
}
