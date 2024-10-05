using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class MoveToTarget : Action
	{
		[field: SerializeField]
		public float Speed { get; private set; }

		[field: SerializeField]
		public float LethalHeight { get; private set; }

		public async override UniTask Perform(OperationContext context, CancellationToken token)
		{
			await UniTask.CompletedTask;
			// Unit unit = context.Caster;
			// UnitPresenter unitPresenter = unit.Presenter;
			// Transform transform = unitPresenter.transform;
			//
			// ParabolicTrajectory3D projectileTrajectory = context.Trajectories.First();
			// Vector3 targetPosition = projectileTrajectory.End;
			//
			// Vector3 direction = (targetPosition - transform.position).normalized;
			// transform.forward = new Vector3(direction.x, 0, direction.z);
			//
			// context.Caster.Locked = true;
			//
			// Vector3 startPosition = transform.position;
			//
			// float time = projectileTrajectory.Parameters.Time;
			// float currentTime = 0f;
			//
			// while (currentTime < time)
			// {
			// 	await UniTask.NextFrame(token);
			//
			// 	currentTime = Mathf.MoveTowards(currentTime, time, Time.deltaTime * Desc.Speed);
			// 	float currentFactor = currentTime / time;
			// 	Vector3 currentPosition = projectileTrajectory.EvaluatePosition(currentFactor);
			//
			// 	transform.position = currentPosition;
			// }
			//
			// unitPresenter.NavMeshAgent.Warp(transform.position);
			//
			// context.Caster.Locked = false;
		}
	}
}
