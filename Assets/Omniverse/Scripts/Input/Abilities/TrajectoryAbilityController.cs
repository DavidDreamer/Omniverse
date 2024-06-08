using System.Threading;
using Bonecrusher.Abilities;
using Cysharp.Threading.Tasks;
using Dreambox.Math;
using Omniverse.Abilities;
using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

[Preserve]
public class TrajectoryAbilityController: TargetAbilityController
{
	[Inject]
	private InputActions.CommonActions CommonActions { get; set; }

	[Inject]
	private AbilityTargetRenderer AbilityTargetRenderer { get; set; }

	[Inject]
	private AbilityRangeRenderer AbilityRangeRenderer { get; set; }

	[Inject]
	private AbilityTrajectoryRenderer TrajectoryRenderer { get; set; }
	
	protected override void Setup(Ability ability)
	{
		base.Setup(ability);
		
		AbilityRangeRenderer.DecalProjector.enabled = true;
		TrajectoryRenderer.gameObject.SetActive(true);
	}
		
	protected override void Cleanup()
	{
		base.Cleanup();
		
		AbilityTargetRenderer.DecalProjector.enabled = false;
		AbilityRangeRenderer.DecalProjector.enabled = false;
		TrajectoryRenderer.gameObject.SetActive(false);
	}

	protected override async UniTask GetTarget(CancellationToken token)
	{
		var trajectoryTarget = (TrajectoryTarget)Ability.Desc.Target;
		AbilityRangeRenderer.SetRange(trajectoryTarget.Range);

		Unit caster = Ability.Unit;
		
		while (true)
		{
			bool navMeshPositionIsValid = NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 targetPosition);
			var targetPositionXZ = new Vector3(targetPosition.x, 0, targetPosition.z);

			Vector3 characterPosition = caster.Presenter.transform.position;
			var characterPositionXZ = new Vector3(characterPosition.x, 0, characterPosition.z);

			Vector3 characterToTarget = targetPositionXZ - characterPositionXZ;

			bool positionIsOutOfRange =
				characterToTarget.sqrMagnitude > trajectoryTarget.Range * trajectoryTarget.Range;

			if (positionIsOutOfRange)
			{
				Vector3 targetPositionClamped =
					characterPosition + characterToTarget.normalized * trajectoryTarget.Range;
				navMeshPositionIsValid =
					NavMesh.SamplePosition(targetPositionClamped, out NavMeshHit navMeshHit, float.MaxValue, 1);
				targetPosition = navMeshHit.position;
			}

			AbilityRangeRenderer.transform.position = characterPosition;
			AbilityTargetRenderer.DecalProjector.enabled = navMeshPositionIsValid;
			AbilityTargetRenderer.transform.position = targetPosition;

			bool trajectoryIsValid = ParabolicTrajectory3D.Create(characterPosition, targetPosition, trajectoryTarget.Height,
				out ParabolicTrajectory3D trajectory);

			TrajectoryRenderer.gameObject.SetActive(trajectoryIsValid);

			if (trajectoryIsValid)
			{
				TrajectoryRenderer.Refresh(trajectory);
			}

			bool abilityCasted = navMeshPositionIsValid && CommonActions.Apply.WasPerformedThisFrame();
			if (abilityCasted)
			{
				Ability.Context.Trajectories.Clear();
				Ability.Context.Trajectories.Add(trajectory);
				return;
			}

			await UniTask.NextFrame(token);
		}
	}
}
