using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class Action : ScriptableObject
	{
		[field: SerializeField]
		public Action Next { get; private set; }

		public abstract UniTask Perform(ExecutionContext context, CancellationToken token);
	}
}
