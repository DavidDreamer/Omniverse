using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class KillUnits : ScriptableObject, IAction<Unit, Unit>, IMultiTargetAction<Unit, Unit>
	{
		public void Perform(Unit actor, Unit target)
		{
			target.Die();
		}

		public void Perform(Unit actor, IEnumerable<Unit> targets)
		{
			foreach (var target in targets)
			{
				target.Die();
			}
		}
	}
}
