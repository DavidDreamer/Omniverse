using System;
using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class MultiAction
	{
		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject[] Actions { get; private set; }

		public void Perform(ActionContext context)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i];

				Unit actor = context.Actor as Unit;

				switch (action)
				{
					case IMultiTargetAction<Unit, Unit> a:
						a.Perform(actor, context.Entities.OfType<Unit>());
						break;
					case IAction<Unit, Unit> a:
						a.Perform(actor, context.Entities.OfType<Unit>().First());
						break;
					case IAction<Unit, Vector3> a:
						a.Perform(actor, context.Vectors.First());
						break;
					case IAction<Unit, ResourceSource> a:
						a.Perform(actor, context.Entities.OfType<ResourceSource>().First());
						break;
					case Action a:
						a.Perform(context);
						break;
				}
			}
		}
	}
}
