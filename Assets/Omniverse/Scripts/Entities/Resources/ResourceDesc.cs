using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Resource")]
	public class ResourceDesc: ScriptableObject
	{
		[field: SerializeField]
		public string Name { get; private set; }
	}
}
