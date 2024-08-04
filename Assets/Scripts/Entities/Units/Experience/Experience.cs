using UnityEngine;

namespace Omniverse.Units
{
	public static class ExperienceUtils
	{
		public static float NormalizedAmountForCurrentLevel(this Experience experience)
		{
			if (experience.Level == experience.Desc.PointsForLevel.Length)
			{
				return 1f;
			}

			int[] levels = experience.Desc.PointsForLevel;
			int level = experience.Level;

			int a = level == 0 ? 0 : levels[level - 1];
			int b = levels[level];

			return Mathf.InverseLerp(a, b, experience.Points);
		}

		public static int PointsForCurrentLevel(this Experience experience)
		{
			int[] pointsForLevel = experience.Desc.PointsForLevel;
			int level = experience.Level;
			return level == 1 ? 0 : pointsForLevel[level - 2];
		}

		public static int PointsForNextLevel(this Experience experience)
		{
			int[] pointsForLevel = experience.Desc.PointsForLevel;
			int level = experience.Level;
			return level > pointsForLevel.Length ? pointsForLevel[level - 2] : pointsForLevel[level - 1];
		}
	}

	public class Experience
	{
		public ExperienceDesc Desc { get; }

		public int Points { get; private set; }

		public int Level { get; private set; }

		public Experience(ExperienceDesc desc)
		{
			Desc = desc;

			Level = 1;
		}

		public void Add(int points)
		{
			Points += points;

			while (Level <= Desc.PointsForLevel.Length && Points >= Desc.PointsForLevel[Level - 1])
			{
				Level++;
			}
		}
	}
}
