using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchMissile : ScriptableObject, IAction<Unit, Vector3>, IAction<Unit, Unit>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public void Perform(Unit actor, Vector3 target)
		{
			actor.SpawnMissile(Missile, target);
		}

		public void Perform(Unit actor, Unit target)
		{
			actor.SpawnMissile(Missile, target);
		}
	}
}