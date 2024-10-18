using System;
using System.Collections.Generic;
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
			Unit actor = context.Actor as Unit;

			var units = context.Entities.OfType<Unit>();
			if (units.Count() > 1)
			{
				Perform(actor, units);
				return;
			}

			if (units.Count() == 1)
			{
				Perform(actor, units.First());
				return;
			}

			var vectors = context.Vectors;
			if (vectors.Count == 1)
			{
				Perform(actor, vectors.First());
				return;
			}

			var resourceSources = context.Entities.OfType<ResourceSource>();
			if (resourceSources.Count() == 1)
			{
				Perform(actor, resourceSources.First());
				return;
			}
		}

		void Perform(Unit actor, Unit unit)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, Unit>;
				action.Perform(actor, unit);
			}
		}

		void Perform(Unit actor, Vector3 vector)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, Vector3>;
				action.Perform(actor, vector);
			}
		}

		void Perform(Unit actor, ResourceSource resourceSource)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, ResourceSource>;
				action.Perform(actor, resourceSource);
			}
		}

		void Perform(Unit actor, IEnumerable<Unit> units)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, IEnumerable<Unit>>;
				action.Perform(actor, units);
			}
		}
	}
}
