using Omniverse;
using Unity.Entities;
using UnityEngine;
/// <summary>
/// TEMP for backward compatability.
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial struct VContainerIntializationSystem : ISystem, ISystemStartStop
{
	public void OnStartRunning(ref SystemState state)
	{
		Object.FindFirstObjectByType<GameScope>().Build();
	}

	public void OnStopRunning(ref SystemState state)
	{
	}
}
