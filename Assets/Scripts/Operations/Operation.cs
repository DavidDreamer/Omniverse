using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public interface IOperation
	{
		void Perform(Entity actor);
	}

	public interface IOperation<TTarget>
	{
		void Perform(Entity actor, TTarget target);
	}

	[Serializable]
	public class Operation<TTarget> : IOperation
	{
		[field: SerializeReference]
		public ITargetProvider<TTarget> TargetProvider { get; private set; }

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

	[Serializable]
	public class Operation<TTargetIn, TTargetOut> : IOperation<TTargetIn>
	{
		[field: SerializeReference]
		public ITargetConverter<TTargetIn, TTargetOut> TargetConverter { get; private set; }

		[field: SerializeReference]
		private IAction<TTargetOut>[] Actions { get; set; }

		public void Perform(Entity actor, TTargetIn targetIn)
		{
			IEnumerable<TTargetOut> targets = TargetConverter.Convert(actor, targetIn);

			for (int i = 0; i < Actions.Length; ++i)
			{
				IAction<TTargetOut> action = Actions[i];

				foreach (TTargetOut target in targets)
				{
					action.Perform(actor, target);
				}
			}
		}
	}
}
