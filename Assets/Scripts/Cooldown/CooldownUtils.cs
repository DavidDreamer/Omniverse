using Unity.Mathematics;

namespace Omniverse
{
	public static class CooldownUtils
	{
		public static bool IsActive(this Cooldown cooldown) => cooldown.TimeLeft > 0f;

		public static float Ratio(this Cooldown cooldown) => math.saturate(cooldown.TimeLeft / cooldown.Time);
	}
}
