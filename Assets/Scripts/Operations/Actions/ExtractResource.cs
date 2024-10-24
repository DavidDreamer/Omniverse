using UnityEngine;

namespace Omniverse
{
	public class ExtractResource : IAction<ResourceSource>
	{
		[field: SerializeField]
		public int Amount { get; private set; }

		public void Perform(Entity actor, ResourceSource target)
		{
			actor.Extract(target, Amount);
		}
	}
}
