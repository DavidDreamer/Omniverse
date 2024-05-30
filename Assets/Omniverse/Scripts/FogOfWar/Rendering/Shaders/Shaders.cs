using System;
using UnityEngine;

namespace Omniverse.FogOfWar.Rendering
{
	[Serializable]
	public class Shaders
	{
		[field: SerializeField]
		public Shader Animate { get; private set; }
		
		[field: SerializeField]
		public Shader Blur { get; private set; }
		
		[field: SerializeField]
		public Shader Apply { get; private set; }
	}
}
