using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IAction
	{
	}

	public interface IAction<TActor, TTarget> : IAction
	{
		void Perform(TActor actor, TTarget target);
	}

	public interface IMultiTargetAction<TActor, TTarget> : IAction
	{
		void Perform(TActor actor, IEnumerable<TTarget> targets);
	}

	public abstract class Action : ScriptableObject, IAction
	{
		public abstract void Perform(ActionContext context);
	}
}
