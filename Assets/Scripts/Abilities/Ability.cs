using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Omniverse
{
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		public void OnUpdate()
		{
			//foreach (var buffer in SystemAPI.Query<DynamicBuffer<Ability>>())
			//{
			//	for (int i = 0; i < buffer.Length; ++i)
			//	{
			//		Ability ability = buffer[i];

			//		ability.ActiveCooldown = Unity.Mathematics.math.max(0f, ability.ActiveCooldown - SystemAPI.Time.DeltaTime);

			//		buffer[i] = ability;
			//	}
			//}
		}
	}

	public struct Ability : IBufferElementData
	{
		public FixedString32Bytes Name;

		public WeakObjectReference<Sprite> Icon;

		public float Cooldown;

		public float ActiveCooldown;

		public bool IsOnCooldown => ActiveCooldown > 0f;

		public float ActiveCooldownRatio => math.saturate(ActiveCooldown / Cooldown);
	}
}
