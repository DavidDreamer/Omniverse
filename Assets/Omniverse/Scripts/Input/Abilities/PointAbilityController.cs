using System.Threading;
using Bonecrusher.Abilities;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Input;
using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

[Preserve]
public class PointAbilityController: TargetAbilityController
{
	[Inject]
	private InputActions.CommonActions CommonActions { get; set; }

	[Inject]
	private AbilityTargetRenderer AbilityTargetRenderer { get; set; }

	[Inject]
	private AbilityRangeRenderer AbilityRangeRenderer { get; set; }

	protected override void Setup(Ability ability)
	{
		base.Setup(ability);

		AbilityRangeRenderer.DecalProjector.enabled = true;
	}

	protected override void Cleanup()
	{
		base.Cleanup();

		AbilityTargetRenderer.DecalProjector.enabled = false;
		AbilityRangeRenderer.DecalProjector.enabled = false;
	}

	protected override async UniTask GetTarget(CancellationToken token)
	{
		Unit caster = Ability.Unit;
		var pointTarget = (PointTarget)Ability.Desc.Target;
		AbilityRangeRenderer.SetRange(pointTarget.Range);

		while (true)
		{
			bool navMeshPositionIsValid = NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 point);
			var targetPositionXZ = new Vector3(point.x, 0, point.z);

			Vector3 characterPosition = caster.Presenter.transform.position;
			var characterPositionXZ = new Vector3(characterPosition.x, 0, characterPosition.z);

			Vector3 characterToTarget = targetPositionXZ - characterPositionXZ;

			bool positionIsOutOfRange = characterToTarget.sqrMagnitude >
			                            pointTarget.Range * pointTarget.Range;

			if (positionIsOutOfRange)
			{
				Vector3 targetPositionClamped =
					characterPosition + characterToTarget.normalized * pointTarget.Range;
				navMeshPositionIsValid =
					NavMesh.SamplePosition(targetPositionClamped, out NavMeshHit navMeshHit, float.MaxValue, 1);
				point = navMeshHit.position;
			}

			AbilityRangeRenderer.transform.position = caster.Presenter.transform.position;
			AbilityTargetRenderer.DecalProjector.enabled = navMeshPositionIsValid;
			AbilityTargetRenderer.transform.position = point;

			bool abilityCasted = navMeshPositionIsValid && CommonActions.Apply.WasPerformedThisFrame();
			if (abilityCasted)
			{
				//TODO
				//Ability.Context.Points.Clear();
				//Ability.Context.Points.Add(point);
				return;
			}

			await UniTask.NextFrame(token);
		}
	}
}
