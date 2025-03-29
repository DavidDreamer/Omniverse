using Unity.NetCode;
using UnityEngine;

namespace Omniverse
{
	public struct AbilityInput : IInputComponentData
	{
		public Vector3 Vector;
		public InputEvent Cast;
	}
}
