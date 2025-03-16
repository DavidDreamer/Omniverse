using Omniverse;
using Omniverse.Mapping;
using Omniverse.Rendering;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TEMP for backward compatability.
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial struct VContainerIntializationSystem : ISystem, ISystemStartStop
{
	public void OnStartRunning(ref SystemState state)
	{
		Object.FindFirstObjectByType<FogOfWarRenderer>(FindObjectsInactive.Include).gameObject.SetActive(true);
		Object.FindFirstObjectByType<MapRenderer>(FindObjectsInactive.Include).gameObject.SetActive(true);
		Object.FindFirstObjectByType<GameScope>().Build();
		Object.FindFirstObjectByType<CanvasScaler>(FindObjectsInactive.Include).gameObject.SetActive(true);
	}

	public void OnStopRunning(ref SystemState state)
	{
	}
}
