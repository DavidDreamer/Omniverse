using UnityEngine;

namespace Omniverse
{
	public abstract class EntityDesc : ScriptableObject
	{
		[field: SerializeField]
		public string Name { get; private set; }

		[field: SerializeField]
		public Sprite Icon { get; private set; }

		[field: SerializeField]
		public GameObject Model { get; private set; }
	}
}
