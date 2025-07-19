using Omniverse;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct GameInitializationSystem : ISystem
{
	private FactionsData factionsData;

	public void OnCreate(ref SystemState state)
	{
		var auth = UnityEngine.Object.FindAnyObjectByType<GameOptionsAuthoring>();

		GameOptions gameOptions = auth.GameOptions;

		state.EntityManager.CreateSingleton(gameOptions);

		MapSettings mapSettings = new()
		{
			Size = gameOptions.MapSize
		};

		state.EntityManager.CreateSingleton(mapSettings);

		FogOfWarSettings fogOfWarSettings = new()
		{
			Mode = gameOptions.FogOfWarMode,
			Size = mapSettings.Size / FogOfWar.Multiplier
		};

		state.EntityManager.CreateSingleton(fogOfWarSettings);

		factionsData = new FactionsData()
		{
			Resources = new NativeHashMap<int, NativeArray<int>>(gameOptions.Factions.Length, Allocator.Persistent)
		};

		for (int i = 0; i < gameOptions.Factions.Length; ++i)
		{
			var resources = new NativeArray<int>(gameOptions.Resources.Length, Allocator.Persistent);
			factionsData.Resources[i] = resources;
		}

		state.EntityManager.CreateSingleton(factionsData);

		var physicsSettings = new PhysicsSettings()
		{
			HitboxLayer = auth.HitboxLayer,
			HitboxLayerMask = auth.HitboxLayerMask
		};

		state.EntityManager.CreateSingleton(physicsSettings);
	}

	public void OnDestroy(ref SystemState state)
	{
		foreach (var resources in factionsData.Resources)
		{
			resources.Value.Dispose();
		}

		factionsData.Resources.Dispose();
	}
}
