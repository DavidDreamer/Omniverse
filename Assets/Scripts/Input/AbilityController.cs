using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Input
{
	public class AbilityController
	{
		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		[Inject]
		private ErrorHandler ErrorHandler { get; set; }

		[Inject]
		private EntityDetector EntityDetector { get; set; }

		public Ability ActiveAbility { get; private set; }

		private CancellationTokenSource CancellationTokenSource { get; set; }

		public void Process(Unit unit, Ability ability)
		{
			if (ActiveAbility == ability)
			{
				Discard();
				return;
			}

			if (ActiveAbility is not null)
			{
				Discard();
			}

			AbilityCastError error = ability.CanBeCasted(unit);

			if (error is not AbilityCastError.None)
			{
				ErrorHandler.Hadle(error.ToString());
				return;
			}

			if (ability.Desc.Target.Type is TargetType.None)
			{
				Cast(unit, ability);
			}
			else
			{
				ActiveAbility = ability;
				CancellationTokenSource = new CancellationTokenSource();
				ProcessAbilityAsync(unit, ability, CancellationTokenSource.Token).Forget();
			}
		}

		private async UniTask ProcessAbilityAsync(Unit unit, Ability ability, CancellationToken token)
		{
			TargetType targetType = ability.Desc.Target.Type;

			if (targetType is TargetType.Point)
			{
				Vector3 point = await ReadWorldPoint(token);
				ability.ExecutionContext.Points.Add(point);
			}
			else if (targetType is TargetType.Direction)
			{
				Vector3 point = await ReadWorldPoint(token);
				Vector3 direction = point - unit.transform.position;
				direction.Set(direction.x, 0, direction.z);
				direction.Normalize();
				ability.ExecutionContext.Directions.Add(direction);
			}
			else if (targetType is TargetType.Unit or TargetType.ResourceSource)
			{
				Entity entity = await ReadEntity(targetType, token);
				ability.ExecutionContext.Entities.Add(entity);
			}

			AbilityCastError error = ability.CanBeCasted(unit);

			if (error is not AbilityCastError.None)
			{
				ErrorHandler.Hadle(error.ToString());
			}
			else
			{
				Cast(unit, ability);
			}

			Discard();
		}

		private void Cast(Unit unit, Ability ability) => unit.Cast(ability, default).Forget();

		private async UniTask<Vector3> ReadWorldPoint(CancellationToken token)
		{
			while (true)
			{
				if (CommonActions.Select.WasPerformedThisFrame())
				{
					bool navMeshPositionIsValid = NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 point);
					if (navMeshPositionIsValid)
					{
						return point;
					}
				}

				await UniTask.NextFrame(token);
			}
		}

		private async UniTask<Entity> ReadEntity(TargetType targetType, CancellationToken token)
		{
			token.Register(Cleanup);

			EntityDetector.ClearFilter();

			if (targetType.HasFlag(TargetType.ResourceSource))
			{
				EntityDetector.AddToFilter<ResourceSource>();
			}

			if (targetType.HasFlag(TargetType.Unit))
			{
				EntityDetector.AddToFilter<Unit>();
			}

			bool inputProcessed;
			bool hasTarget;

			do
			{
				await UniTask.NextFrame(token);

				inputProcessed = CommonActions.Select.WasPressedThisFrame();
				hasTarget = EntityDetector.Target != null;
			}
			while (!inputProcessed || !hasTarget);

			Cleanup();

			return EntityDetector.Target;

			void Cleanup() => EntityDetector.SetDefaultDetectableType();
		}

		private void Discard()
		{
			ActiveAbility = null;
			CancellationTokenSource.Cancel();
		}
	}
}
