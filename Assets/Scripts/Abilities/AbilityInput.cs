using Unity.NetCode;
using UnityEngine;

namespace Omniverse
{
	public struct AbilityInput : IInputComponentData
	{
		public int AbilityIndex;
		public Vector3 Vector;
		public InputEvent Cast;
	}
}
