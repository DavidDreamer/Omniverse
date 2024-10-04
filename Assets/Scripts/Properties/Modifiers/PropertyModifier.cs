using System;
using Dreambox.Core;

namespace Omniverse
{
	[Serializable]
	public struct PropertyModifier
	{
		public ArithmeticOperation Operation;

		public PropertyModifierMode Mode;

		public float Value;
	}
}
