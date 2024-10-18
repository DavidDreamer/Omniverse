using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ExtractResource : Action<Unit, ResourceSource>
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public override void Perform(Unit actor, ResourceSource target)
		{
			actor.Extract(target, Amount);
		}
	}
}
