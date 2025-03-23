using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
	public partial struct RenderingInitializationSystem : ISystem, ISystemStartStop
	{
		public void OnStartRunning(ref SystemState state)
		{
			Object.FindFirstObjectByType<RenderingClient>(FindObjectsInactive.Include).gameObject.SetActive(true);
			Object.FindFirstObjectByType<CanvasScaler>(FindObjectsInactive.Include).gameObject.SetActive(true);
		}

		public void OnStopRunning(ref SystemState state)
		{
		}
	}
}
