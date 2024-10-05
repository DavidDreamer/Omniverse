using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;
using Object = UnityEngine.Object;

namespace Omniverse.Actions
{
	public class SpawnVisualEffect : Action
	{
		[field: SerializeField]
		public VisualEffect VisualEffect { get; private set; }

		[field: SerializeField]
		public float Time { get; private set; }

		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			Vector3 position = context.Points.First();

			VisualEffect visualEffect = Object.Instantiate(VisualEffect, position, Quaternion.identity);

			Object.Destroy(visualEffect, Time);

			return UniTask.CompletedTask;
		}
	}
}
