using System;
using UnityEngine;

namespace Omniverse.UI
{
	[Serializable]
	public class TextStyle
	{
		[field: SerializeField]
		public Color DefaultColor { get; private set; }

		[field: SerializeField]
		public Color PositiveColor { get; private set; }

		[field: SerializeField]
		public Color NegativeColor { get; private set; }
	}
}
