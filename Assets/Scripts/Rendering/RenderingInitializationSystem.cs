using Unity.Entities;
using UnityEngine;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.Presentation)]
	public partial struct RenderingInitializationSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.HasSingleton<Player>())
			{
				var rendering = Object.FindFirstObjectByType<RenderingClient>(FindObjectsInactive.Include);
				rendering.Initialize(state.EntityManager);
				rendering.gameObject.SetActive(true);
				state.Enabled = false;
			}
		}
	}
}
