using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class ExperienceDesc
	{
		[field: SerializeField]
		public int[] PointsForLevel { get; private set; }
	}
}
