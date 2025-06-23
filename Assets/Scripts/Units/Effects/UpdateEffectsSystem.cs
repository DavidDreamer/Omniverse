using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct UpdateEffectsSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((var _, var entity) in SystemAPI.Query<DynamicBuffer<Effect>>().WithEntityAccess())
			{
				var buffer = SystemAPI.GetBuffer<Effect>(entity);

				for (int i = 0; i < buffer.Length; i++)
				{
					Effect effect = buffer[i];

					effect.Time -= deltaTime;

					buffer[i] = effect;

					if (effect.Time <= 0)
					{
						buffer.RemoveAt(i);
						i--;
					}
				}
			}
		}
	}
}
