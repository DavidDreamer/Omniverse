using Omniverse;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerInitializationSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingleton<Player>(nameof(Player));
	}
}
