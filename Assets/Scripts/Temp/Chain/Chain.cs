using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class Chain : MonoBehaviour
	{
		private float Time { get; set; }

		private ChainDesc Desc { get; set; }

		public DynamicEntity Target { get; set; }

		public DynamicEntity Owner { get; set; }

		public HashSet<DynamicEntity> Targets { get; } = new();

		public void Initialize(ChainDesc desc)
		{
			Desc = desc;
		}

		public void Tick(float deltaTime)
		{
			//TODO ECS
			//if (Time == 0)
			//{
			//	Targets.Add(Target);
			//	Desc.Operation.Perform(Owner, Target);
			//}

			//Time += deltaTime;

			//if (Time >= Desc.BounceInterval)
			//{
			//	if (Targets.Count == Desc.MaxTargets)
			//	{
			//		Completed = true;
			//		return;
			//	}

			//	Vector3 position = Target.transform.position;
			//	Target = null;
			//	float minDistance = float.MaxValue;

			//	foreach (var target in PhysicsService.GetEntitiesInSphere<UnitObsolete>(Owner, Desc.BounceRange, Desc.Filter))
			//	{
			//		if (target == Owner)
			//		{
			//			continue;
			//		}

			//		if (Targets.Contains(target))
			//		{
			//			continue;
			//		}

			//		float distance = Vector3.SqrMagnitude(target.transform.position - position);

			//		if (distance >= minDistance)
			//		{
			//			continue;
			//		}

			//		Target = target;
			//		minDistance = distance;
			//	}

			//	if (Target == null)
			//	{
			//		Completed = true;
			//	}

			//	Time = 0;
			//}
		}
	}
}
