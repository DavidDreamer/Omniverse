using UnityEngine;

namespace Omniverse
{
	public class Effect
	{
		private Entity Entity { get; }

		public EffectDesc Desc { get; }

		public float Time { get; private set; }

		public bool OutOfTime => Time == 0;

		public Effect(Entity entity, EffectDesc desc)
		{
			Entity = entity;
			Desc = desc;
			Time = desc.Time;
		}

		public void Tick(float deltaTime)
		{
			Desc.OnTickOperation?.Perform(Entity, None.Instance);
			Time = Mathf.Max(0, Time - deltaTime);
		}
	}
}
