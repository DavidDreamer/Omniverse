using System;
using UnityEngine;

namespace Omniverse.Rendering
{
	[Serializable]
	public struct FogOfWarProperties
	{
		public Color UnexploredColor;

		public Color ExploredColor;

		public float AnimationSpeed;

		public float BorderLength;
	}
}
