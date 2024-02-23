using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dreambox.Math;
using JetBrains.Annotations;
using UnityEngine;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class MoveToTarget: Action<MoveToTargetDesc>
	{
		public MoveToTarget(MoveToTargetDesc desc): base(desc)
		{
		}
		
		public override async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			Unit unit = context.Caster;
			UnitPresenter unitPresenter = unit.Presenter;
			Transform transform = unitPresenter.transform;
			
			ParabolicTrajectory3D projectileTrajectory = context.Trajectories.First();
			Vector3 targetPosition = projectileTrajectory.End;

			Vector3 direction = (targetPosition - transform.position).normalized;
			transform.forward = new Vector3(direction.x, 0, direction.z);

			context.Caster.Locked = true;

			Vector3 startPosition = transform.position;

			float time = projectileTrajectory.Parameters.Time;
			float currentTime = 0f;

			while (currentTime < time)
			{
				await UniTask.NextFrame(token);

				currentTime = Mathf.MoveTowards(currentTime, time, Time.deltaTime * Desc.Speed);
				float currentFactor = currentTime / time;
				Vector3 currentPosition = projectileTrajectory.EvaluatePosition(currentFactor);

				transform.position = currentPosition;
			}

			unitPresenter.NavMeshAgent.Warp(transform.position);
			
			context.Caster.Locked = false;

			float deltaHeight = startPosition.y - targetPosition.y;
			if (deltaHeight >= Desc.LethalHeight)
			{
				var damage = new ChangeResourceData
				{
					ResourceID = 0,
					Amount = -1
				};

				context.Caster.ChangeResource(damage);
			}
		}
	}
}
