using Unity.Mathematics;

namespace Omniverse
{
	public static class CooldownUtils
	{
		public static float Ratio(this Cooldown cooldown) => math.saturate(cooldown.TimeLeft / cooldown.Duration);
	}
}
