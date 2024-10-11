using UnityEngine;

namespace Omniverse.Abilities
{
	public class Cooldown
	{
		public CooldownDesc Desc { get; }

		public float TimeLeft { get; private set; }

		public float TimeLeftRatio { get; private set; }

		public bool IsActive => TimeLeft > 0f;

		public Cooldown(CooldownDesc desc)
		{
			Desc = desc;
		}

		public void Tick(float deltaTime)
		{
			if (TimeLeft == 0f)
			{
				return;
			}

			TimeLeft = Mathf.Max(0, TimeLeft - deltaTime);
			TimeLeftRatio = Mathf.Clamp01(TimeLeft / Desc.Time);
		}

		public void Activate()
		{
			TimeLeft = Desc.Time;
		}
	}
}
