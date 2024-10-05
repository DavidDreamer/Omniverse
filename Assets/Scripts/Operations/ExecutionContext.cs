using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dreambox.Math;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ExecutionContext
	{
		public Entity Caster { get; set; }

		public List<Entity> Entities { get; } = new();

		public List<Vector3> Points { get; } = new();

		public List<Vector3> Directions { get; } = new();

		public List<ParabolicTrajectory3D> Trajectories { get; } = new();

		public async UniTask PerformAsync(Operation operation, Entity caster, CancellationToken token)
		{
			Caster = caster;

			do
			{
				operation = await operation.PerformAsync(this, token);
			}
			while (operation != null);

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
