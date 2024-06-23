using System;
using UnityEngine;

namespace Omniverse.Entities.Units.Rendering
{
	[Serializable]
	public class HealthBarColors
	{
		[field: SerializeField]
		public Color BaseColor { get; private set; }
		
		[field: SerializeField]
		public Color SecondColor { get; private set; }
	}
}
