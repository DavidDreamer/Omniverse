using Omniverse;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerInitializationSystem : ISystem, ISystemStartStop
{
	public void OnStartRunning(ref SystemState state)
	{
		var gameOptions = SystemAPI.ManagedAPI.GetSingleton<GameOptions>();

		var factionsData = new FactionsData()
		{
			Resources = new NativeHashMap<int, NativeArray<int>>(gameOptions.Factions.Length, Allocator.Persistent)
		};

		for (int i = 0; i < gameOptions.Factions.Length; ++i)
		{
			var resources = new NativeArray<int>(gameOptions.Resources.Length, Allocator.Persistent);
			factionsData.Resources[i] = resources;
		}

		state.EntityManager.CreateSingleton(factionsData);

		state.EntityManager.CreateSingleton<Player>(nameof(Player));
	}

	public void OnStopRunning(ref SystemState state)
	{
	}
}
