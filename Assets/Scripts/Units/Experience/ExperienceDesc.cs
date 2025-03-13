using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ExperienceDesc
	{
		[field: SerializeField]
		public int[] PointsForLevel { get; private set; }
	}
}
