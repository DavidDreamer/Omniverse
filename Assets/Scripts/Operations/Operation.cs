using System;
using System.Collections.Generic;
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
			IEnumerable<TTarget> targets = TargetProvider.Get(actor);

			for (int i = 0; i < Actions.Length; ++i)
			{
				IAction<TTarget> action = Actions[i];

				foreach (TTarget target in targets)
				{
					action.Perform(actor, target);
				}
			}
		}
	}
}
