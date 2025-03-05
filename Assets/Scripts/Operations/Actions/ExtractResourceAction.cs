using UnityEngine;

namespace Omniverse
{
	public class ExtractResourceAction : IAction<ResourceSource>
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public void Perform(OmniverseEntity actor, ResourceSource target)
		{
			actor.Extract(target, Amount);
		}
	}
}
