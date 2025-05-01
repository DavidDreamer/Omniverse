using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
	public partial struct RenderingInitializationSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.HasSingleton<Player>())
			{
				Object.FindFirstObjectByType<RenderingClient>(FindObjectsInactive.Include).gameObject.SetActive(true);

				state.Enabled = false;
			}
		}
	}
}
