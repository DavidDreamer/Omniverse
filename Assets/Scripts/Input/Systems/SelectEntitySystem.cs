using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputActions;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	[UpdateAfter(typeof(ProcessPointerSystem))]
	public partial struct SelectEntitySystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			Selection selection = new()
			{
				Entities = new(Selection.Capacity, Allocator.Persistent)
			};

			state.EntityManager.CreateSingleton(selection);
		}

		public void OnUpdate(ref SystemState state)
		{
			EntityManager entityManager = state.EntityManager;

			var player = SystemAPI.GetSingleton<Player>();
			var detector = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingletonRW<Selection>();

			var inputSystemData = SystemAPI.ManagedAPI.GetSingleton<InputSystemData>();
			CommonActions commonActions = inputSystemData.InputActions.Common;
			InputAction selectAction = commonActions.Select;
			Mouse mouse = Mouse.current;
			Vector2 mousePosition = Mouse.current.position.value;
			Camera camera = Camera.main;

			if (selectAction.WasPressedThisFrame())
			{
				selection.ValueRW.StartPosition = mouse.position.value;
				selection.ValueRW.InProcess = true;
			}

			if (selectAction.IsPressed())
			{
				selection.ValueRW.EndPosition = mouse.position.value;
			}

			if (selection.ValueRW.InProcess && selectAction.WasReleasedThisFrame())
			{
				selection.ValueRW.InProcess = false;

				bool additiveMode = commonActions.AdditiveMode.IsPressed();

				if (!additiveMode)
				{
					ClearSelection();
				}

				if (Vector2.Distance(selection.ValueRW.StartPosition, selection.ValueRW.EndPosition) > Selection.SelectionBoxTreshold)
				{
					Vector2 start = (Vector2)selection.ValueRW.StartPosition;
					Vector2 end = (Vector2)selection.ValueRW.EndPosition;

					foreach (Entity entity in PhysicsService.GetEntitiesInScreenRect(camera, start, end))
					{
						var faction = entityManager.GetSharedComponent<Faction>(entity);

						if (faction.ID != player.FactionID)
						{
							continue;
						}

						TrySelect(entity);
					}
				}
				else
				{
					TrySelectSingleTarget(detector.Entity);
				}
			}

			UpdateSelectionIndex();

			void TrySelectSingleTarget(Entity entity)
			{
				if (entity == Entity.Null)
				{
					return;
				}

				bool isSelected = selection.ValueRW.Entities.Contains(entity);

				if (isSelected)
				{
					RemoveFromSelection(entity);
				}
				else
				{
					TrySelect(entity);
				}
			}

			void UpdateSelectionIndex()
			{
				if (commonActions.NextSelectionTarget.WasReleasedThisFrame())
				{
					selection.ValueRW.SelectionIndex++;
					if (selection.ValueRW.SelectionIndex >= selection.ValueRW.Entities.Length)
					{
						selection.ValueRW.SelectionIndex = 0;
					}
				}
			}

			void TrySelect(Entity entity)
			{
				if (selection.ValueRW.Entities.Contains(entity))
				{
					return;
				}

				if (selection.ValueRW.Entities.Length == Selection.Capacity)
				{
					return;
				}

				selection.ValueRW.Entities.Add(entity);
			}

			void RemoveFromSelection(Entity entity)
			{
				selection.ValueRW.Entities.RemoveAt(selection.ValueRW.Entities.IndexOf(entity));
			}

			void ClearSelection()
			{
				selection.ValueRW.Entities.Clear();
				selection.ValueRW.SelectionIndex = 0;
			}
		}
	}
}
