using System;
using UnityEngine;

namespace Omniverse.Client
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
