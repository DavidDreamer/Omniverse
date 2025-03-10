using Omniverse;
using Omniverse.Entities.Units;
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

		foreach (var spawner in Object.FindObjectsByType<UnitSpawner>(FindObjectsSortMode.None))
		{
			spawner.Spawn();
		}

		Physics.simulationMode = SimulationMode.FixedUpdate;
	}

	public void OnStopRunning(ref SystemState state)
	{
	}
}
