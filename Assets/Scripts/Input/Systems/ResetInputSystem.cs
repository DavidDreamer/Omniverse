using Unity.Burst;
using Unity.Entities;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup), OrderFirst = true)]
	public partial struct ResetInputSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var input in SystemAPI.Query<RefRW<AbilityInput>>())
			{
				input.ValueRW.Cast = default;
			}
		}
	}
}
