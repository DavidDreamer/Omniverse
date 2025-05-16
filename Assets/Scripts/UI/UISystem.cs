using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	[WorldSystemFilter(WorldSystemFilterFlags.Presentation)]
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial class UISystem : SystemBase
	{
		private UIHandler UIHandler;

		protected override void OnUpdate()
		{
			if (UIHandler == null)
			{
				if (SystemAPI.HasSingleton<Player>())
				{
					UIHandler = Object.FindFirstObjectByType<UIHandler>(FindObjectsInactive.Include);
					UIHandler.Initialize(EntityManager);
				}
				else
				{
					return;
				}
			}

			UIHandler.Tick();
		}
	}
}
