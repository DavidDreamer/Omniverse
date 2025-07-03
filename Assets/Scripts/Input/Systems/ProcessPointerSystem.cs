using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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

			Camera camera = Camera.main;

			if (camera == null)
			{
				return;
			}

			Mouse mouse = Mouse.current;
			Vector2 mousePosition = Mouse.current.position.value;
			UnityEngine.Ray ray = camera.ScreenPointToRay(mousePosition);

			var pointer = SystemAPI.GetSingletonRW<Pointer>();
			var builder = SystemAPI.GetSingleton<Builder>();
			pointer.ValueRW.Entity = Entity.Null;

			if (builder.InProcess)
			{
				if (NavmeshUtils.GetNavMeshPositionFromCursor(ray, out Vector3 position))
				{
					pointer.ValueRW.CellPosiiton = GetNearestCellPosition(position);
					Vector3 cellScreenPoint = camera.WorldToScreenPoint(pointer.ValueRW.CellPosiiton);
					Vector2 cursorPosition = new(cellScreenPoint.x, cellScreenPoint.y);
					//TODO better snapping
					//mouse.WarpCursorPosition(cursorPosition);
				}
			}
			else
			{
				bool pointerIsOverUI = EventSystem.current.IsPointerOverGameObject();

				if (pointerIsOverUI)
				{
					pointer.ValueRW.TargetType = PointerTargetType.UI;
				}
				else
				{
					var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
					var physicsSettings = SystemAPI.GetSingleton<PhysicsSettings>();

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
						pointer.ValueRW.TargetType = PointerTargetType.Entity;
						pointer.ValueRW.Entity = raycastHit.Entity;
					}
					else
					{
						if (NavmeshUtils.GetNavMeshPositionFromCursor(ray, out Vector3 position))
						{
							pointer.ValueRW.TargetType = PointerTargetType.World;
							pointer.ValueRW.WorldPosition = position;
						}
						else
						{
							pointer.ValueRW.TargetType = PointerTargetType.None;
						}
					}
				}
			}

			float3 GetNearestCellPosition(float3 worldPosition)
			{
				float x = Mathf.Floor(worldPosition.x) + 0.5f;
				float z = Mathf.Floor(worldPosition.z) + 0.5f;
				return new(x, worldPosition.y, z);
			}
		}
	}
}
