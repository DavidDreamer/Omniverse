using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ExtractResource : ScriptableObject, IAction<Unit, ResourceSource>
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public void Perform(Unit actor, ResourceSource target)
		{
			actor.Extract(target, Amount);
		}
	}
}
