using System;
using UnityEngine;

namespace Omniverse.Rendering
{
	[Serializable]
	public class CursorParams
	{
		[field: SerializeField]
		public Texture2D Texture { get; private set; }

		[field: SerializeField]
		public Vector2 Hotspot { get; private set; }
	}
}