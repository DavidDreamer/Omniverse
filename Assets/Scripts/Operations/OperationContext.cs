using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class OperationContext
	{
		public Entity Actor { get; }

		public List<Entity> Entities { get; } = new();

		public List<Vector3> Points { get; } = new();

		public List<Vector3> Directions { get; } = new();

		[Inject]
		public PhysicsService PhysicsService { get; private set; }

		[Inject]
		public IObjectResolver ObjectResolver { get; private set; }

		public OperationContext(Entity actor)
		{
			Actor = actor;
		}

		public void Clear()
		{
			Entities.Clear();
			Points.Clear();
			Directions.Clear();
		}
	}
}
