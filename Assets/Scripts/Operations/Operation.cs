using System;
using UnityEngine;

namespace Omniverse
{
	public interface IOperation
	{
		void Perform(Entity actor);
	}

	[Serializable]
	public class Operation<TTarget> : IOperation
	{
		[field: SerializeReference]
		public ITargetProvider<TTarget> TargetProvider { get; set; }

		[field: SerializeReference]
		private IAction<TTarget>[] Actions { get; set; }

		public void Perform(Entity actor)
		{
			TTarget target = TargetProvider.Get(actor);
			for (int i = 0; i < Actions.Length; ++i)
			{
				Actions[i].Perform(actor, target);
			}
		}
	}
}
