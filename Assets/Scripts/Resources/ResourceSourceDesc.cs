using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Resource Source")]
	public class ResourceSourceDesc : ScriptableObject
	{
		[field: SerializeField]
		public ResourceDesc Resource { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
