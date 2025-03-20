using Omniverse.Mapping;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.Rendering
{
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
	public partial struct RenderingInitializationSystem : ISystem, ISystemStartStop
	{
		public void OnStartRunning(ref SystemState state)
		{
			Object.FindFirstObjectByType<FogOfWarRenderer>(FindObjectsInactive.Include).gameObject.SetActive(true);
			Object.FindFirstObjectByType<MapRenderer>(FindObjectsInactive.Include).gameObject.SetActive(true);
			Object.FindFirstObjectByType<CanvasScaler>(FindObjectsInactive.Include).gameObject.SetActive(true);
		}

		public void OnStopRunning(ref SystemState state)
		{
		}
	}
}
