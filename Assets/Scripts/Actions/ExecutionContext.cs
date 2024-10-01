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
		private IActionDesc[] Actions { get; }

		public Entity Caster { get; set; }

		public List<Entity> Entities { get; } = new();

		public List<Vector3> Points { get; } = new();

		public List<Vector3> Directions { get; } = new();

		public List<ParabolicTrajectory3D> Trajectories { get; } = new();

		public ExecutionContext(IObjectResolver objectResolver, IActionDesc[] actionDescs)
		{
			Actions = actionDescs;

			foreach (var action in Actions)
			{
				objectResolver.Inject(action);
			}
		}

		public async UniTask PerformAsync(Entity caster, CancellationToken token)
		{
			Caster = caster;

			foreach (IActionDesc action in Actions)
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
			Directions.Clear();
			Trajectories.Clear();
		}
	}
}
