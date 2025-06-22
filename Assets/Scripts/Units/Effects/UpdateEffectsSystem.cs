using Unity.Burst;
using Unity.Entities;

namespace Omniverse
{
	[BurstCompile]
	public partial struct UpdateEffectsSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((var buffer, var entity) in SystemAPI.Query<DynamicBuffer<Effect>>().WithEntityAccess())
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					Effect effect = buffer[i];

					effect.Time -= deltaTime;

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
