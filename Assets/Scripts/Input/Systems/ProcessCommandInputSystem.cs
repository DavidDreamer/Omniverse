using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static InputActions;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessCommandInputSystem : ISystem
	{
		public static event Action<Vector3> NavigationPointCreated;

		public void OnUpdate(ref SystemState state)
		{
			var inputSystemData = SystemAPI.ManagedAPI.GetSingleton<InputSystemData>();
			var pointer = SystemAPI.GetSingleton<Pointer>();
			CommonActions commonActions = inputSystemData.InputActions.Common;

			if (commonActions.Command.WasReleasedThisFrame())
			{
				switch (pointer.TargetType)
				{
					//case PointerTargetType.Entity:
					//foreach (UnitObsolete selectedUnit in Selector.SelectedUnits)
					//{
					//	if (selectedUnit != unit)
					//	{
					//		if (unit.IsAllyFor(selectedUnit))
					//		{
					//			var command = new FollowCommand(selectedUnit, unit);
					//			AddCommand(selectedUnit, command);
					//		}
					//		else
					//		{
					//			var command = new AttackCommand(selectedUnit, unit);
					//			AddCommand(selectedUnit, command);
					//		}
					//	}
					//}
					//break;
					//case Item item:
					//	foreach (UnitObsolete selectedUnit in Selector.SelectedUnits)
					//	{
					//		var command = new PickUpItemCommand(selectedUnit, item);
					//		AddCommand(selectedUnit, command);
					//	}
					//	break;
					case PointerTargetType.World:
						var selection = SystemAPI.GetSingleton<Selection>();
						foreach (Entity entity in selection.Entities)
						{
							var agent = SystemAPI.GetComponent<NavAgentComponent>(entity);
							agent.targetPosition = pointer.WorldPosition;
							SystemAPI.SetComponent(entity, agent);
						}
						NavigationPointCreated?.Invoke(pointer.WorldPosition);
						break;
				}
			}
			//else if (commonActions.Stop.WasPerformedThisFrame())
			//{
			//	foreach (UnitObsolete unit in Selector.SelectedUnits)
			//	{
			//		unit.CommandModule.Reset();
			//	}
			//}

			void AddCommand(UnitObsolete unit, ICommand command)
			{
				if (!commonActions.AdditiveMode.IsPressed())
				{
					unit.CommandModule.Reset();
				}

				unit.CommandModule.Add(command);
			}
		}

		public void ProcessNavigationPoint(Vector3 position)
		{
			if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, float.MaxValue, 1))
			{
				CreateNavigationPoint(navMeshHit.position);
			}
		}

		private void CreateNavigationPoint(Vector3 position)
		{
			//foreach (UnitObsolete unit in Selector.SelectedUnits)
			//{
			//	MoveCommand moveCommand = new(unit, position);
			//	AddCommand(unit, moveCommand);
			//}

		

			//NavigationPointCreated?.Invoke(position);
		}
	}
}
