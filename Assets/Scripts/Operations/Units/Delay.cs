using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Delay : Action
	{
		[field: SerializeField]
		public float Duration { get; private set; }

		public override async UniTask Perform(OperationContext context, CancellationToken token) =>
			await UniTask.Delay(TimeSpan.FromSeconds(Duration), cancellationToken: token);
	}
}
