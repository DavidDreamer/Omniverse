using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class ActionContext
	{
		public Entity Actor { get; }

		public List<Entity> Entities { get; } = new();

		public List<Vector3> Vectors { get; } = new();

		[Inject]
		public PhysicsService PhysicsService { get; private set; }

		[Inject]
		public IObjectResolver ObjectResolver { get; private set; }

		[Inject]
		public ResourceExtractionHadler ResourceExtractionHadler { get; set; }

		public ActionContext(Entity actor)
		{
			Actor = actor;
		}

		public void Clear()
		{
			Entities.Clear();
			Vectors.Clear();
		}
	}
}
