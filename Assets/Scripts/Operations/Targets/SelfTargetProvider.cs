using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetProvider<Unit>, ITargetProvider<Vector3>
	{
		Unit ITargetProvider<Unit>.Get(Unit actor) => actor;

		Vector3 ITargetProvider<Vector3>.Get(Unit actor) => actor.transform.position;
	}
}
