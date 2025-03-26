using Omniverse.Abilities;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach (var cooldown in SystemAPI.Query<RefRW<Cooldown>>())
			{
				cooldown.ValueRW.TimeLeft = math.max(0f, cooldown.ValueRW.TimeLeft - deltaTime);
			}
		}
	}

	public struct CastRange : IComponentData
	{
		public float Value;
	}

	public class AbilityTarget : IComponentData
	{
		public ITarget Target;
	}

	public struct Ability : IComponentData
	{
	}
}
