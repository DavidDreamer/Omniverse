using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
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
			CommonActions commonActions = inputSystemData.InputActions.Common;

			//TODO ECS
			//if (commonActions.Command.WasReleasedThisFrame())
			//{
			//	if (target != null)
			//	{
			//		switch (target)
			//		{
			//			case UnitObsolete unit:
			//				foreach (UnitObsolete selectedUnit in Selector.SelectedUnits)
			//				{
			//					if (selectedUnit != unit)
			//					{
			//						if (unit.IsAllyFor(selectedUnit))
			//						{
			//							var command = new FollowCommand(selectedUnit, unit);
			//							AddCommand(selectedUnit, command);
			//						}
			//						else
			//						{
			//							var command = new AttackCommand(selectedUnit, unit);
			//							AddCommand(selectedUnit, command);
			//						}
			//					}
			//				}
			//				break;
			//			case Item item:
			//				foreach (UnitObsolete selectedUnit in Selector.SelectedUnits)
			//				{
			//					var command = new PickUpItemCommand(selectedUnit, item);
			//					AddCommand(selectedUnit, command);
			//				}
			//				break;
			//		}
			//	}
			//	else
			//	{
			//		if (position.HasValue)
			//		{
			//			CreateNavigationPoint(position.Value);
			//		}
			//	}
			//}
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
