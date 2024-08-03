using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dreambox.Math;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	public class ExecutionContext
	{
		private IAction[] Actions { get; }

		public Entity Caster { get; set; }

		public List<Entity> Entities { get; } = new();

		public List<Vector3> Points { get; } = new();

		public List<ParabolicTrajectory3D> Trajectories { get; } = new();
		
		public ExecutionContext(IObjectResolver objectResolver, IActionDesc[] actionDescs)
		{
			Actions = new IAction[actionDescs.Length];

			for (int i = 0; i < actionDescs.Length; ++i)
			{
				IAction action = actionDescs[i].Build();
				objectResolver.Inject(action);
				Actions[i] = action;
			}
		}

		public async UniTask PerformAsync(Entity caster, CancellationToken token)
		{
			Caster = caster;
			
			foreach (IAction action in Actions)
			{
				await action.Perform(this, token);
			}

			Clear();
		}

		private void Clear()
		{
			Caster = null;
			Entities.Clear();
			Points.Clear();
			Trajectories.Clear();
		}
	}
}
