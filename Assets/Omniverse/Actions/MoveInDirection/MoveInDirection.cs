﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse.Actions
{
	[Serializable]
	public class MoveInDirection: Action<MoveInDirectionDesc>
	{
		public MoveInDirection(MoveInDirectionDesc desc): base(desc)
		{
		}

		public override async UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			Transform transform = context.Caster.Presenter.transform;

			Vector3 worldSpaceDirection = transform.rotation * Desc.Direction;
			Vector3 sourcePoint = transform.position;
			Vector3 targetPoint = sourcePoint + worldSpaceDirection * Desc.Distance;
			if (NavMesh.SamplePosition(targetPoint, out NavMeshHit h, float.MaxValue, 1))
			{
				targetPoint = h.position;
			}

			float time = 0f;

			while (time < Desc.Duration)
			{
				await UniTask.WaitForFixedUpdate(token);

				time = Mathf.Min(time + Time.fixedDeltaTime, Desc.Duration);

				Vector3 currentPosition = Vector3.Lerp(sourcePoint, targetPoint, time / Desc.Duration);

				if (NavMesh.SamplePosition(currentPosition, out NavMeshHit hit, float.MaxValue, 1))
				{
					currentPosition = hit.position;
				}

				transform.position = currentPosition;
			}
		}
	}
}
