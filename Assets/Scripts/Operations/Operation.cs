using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public interface IOperation
	{
	}

	public interface IOperation<TTarget> : IOperation
	{
		void Perform(EntityManager entityManager, Entity actor, TTarget target);
	}

	[Serializable]
	public class Operation<TTargetIn, TTargetOut> : IOperation<TTargetIn>
	{
		[field: SerializeReference]
		public ITargetConverter<TTargetIn, TTargetOut> TargetConverter { get; private set; }

		[field: SerializeReference]
		private IAction<TTargetOut>[] Actions { get; set; }

		public void Perform(EntityManager entityManager, Entity actor, TTargetIn targetIn)
		{
			IEnumerable<TTargetOut> targets = TargetConverter.Convert(entityManager, actor, targetIn);

			for (int i = 0; i < Actions.Length; ++i)
			{
				IAction<TTargetOut> action = Actions[i];

				foreach (TTargetOut target in targets)
				{
					action.Perform(entityManager, actor, target);
				}
			}
		}
	}
}
