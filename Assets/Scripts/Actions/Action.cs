using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class Action : ScriptableObject
	{
		[field: SerializeField]
		[field: ActionPicker]
		public Action Then { get; private set; }

		public abstract UniTask Perform(ExecutionContext context, CancellationToken token);
	}
}
