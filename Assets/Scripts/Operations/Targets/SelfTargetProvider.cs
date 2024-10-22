using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetProvider<Unit>, ITargetProvider<Vector3>
	{
		Unit ITargetProvider<Unit>.Get(Entity actor) => (Unit)actor;

		Vector3 ITargetProvider<Vector3>.Get(Entity actor) => actor.transform.position;
	}
}
