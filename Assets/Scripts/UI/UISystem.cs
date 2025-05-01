using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	[WorldSystemFilter(WorldSystemFilterFlags.Presentation)]
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	public partial class UISystem : SystemBase
	{
		private UIHandler UIHandler;

		protected override void OnCreate()
		{
			UIHandler = Object.FindFirstObjectByType<UIHandler>(FindObjectsInactive.Include);
			UIHandler.Initialize(EntityManager);

			RequireForUpdate<Player>();
		}

		protected override void OnUpdate()
		{
			UIHandler.Tick();
		}
	}
}
