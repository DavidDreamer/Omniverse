using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Units;
using UnityEngine;
using VContainer;
using static InputActions;

namespace Omniverse.Input
{
	public class DirectionAbilityHandler : TargetAbilityHandler
	{
		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		protected override async UniTask GetTarget(Unit caster, CancellationToken token)
		{
			while (true)
			{
				bool navMeshPositionIsValid = NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 point);

				Vector3 direction = point - caster.transform.position;
				direction.Set(direction.x, 0, direction.z);
				direction.Normalize();

				if (navMeshPositionIsValid && CommonActions.Apply.WasPerformedThisFrame())
				{
					Ability.ExecutionContext.Directions.Add(direction);
					return;
				}

				await UniTask.NextFrame(token);
			}
		}
	}
}