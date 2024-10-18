using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	public interface IAction
	{
	}

	public abstract class Action<TActor, TTarget> : ScriptableObject, IAction
	{
		public abstract void Perform(TActor actor, TTarget target);
	}

	public abstract class Action : ScriptableObject, IAction
	{
		public abstract void Perform(Unit actor);
	}
}
