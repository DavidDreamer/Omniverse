using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Omniverse
{
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (var buffer in SystemAPI.Query<DynamicBuffer<Ability>>())
			{
				var bufferCopy = buffer;

				for (int i = 0; i < buffer.Length; ++i)
				{
					Ability ability = buffer[i];
					Cooldown cooldown = ability.Cooldown;

					cooldown.TimeLeft = math.max(0f, cooldown.TimeLeft - SystemAPI.Time.DeltaTime);
					ability.Cooldown = cooldown;

					bufferCopy[i] = ability;
				}
			}
		}
	}

	public struct Ability : IBufferElementData
	{
		public FixedString32Bytes Name;

		public WeakObjectReference<Sprite> Icon;

		public Cooldown Cooldown;

		public float CastRange;
	}
}
