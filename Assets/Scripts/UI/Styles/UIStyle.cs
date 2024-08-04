using UnityEngine;

namespace Omniverse.UI
{
	[CreateAssetMenu(menuName = "Omniverse/UI/Style")]
	public class UIStyle : ScriptableObject
	{
		[field: SerializeField]
		public TextStyle Text { get; private set; }
	}
}
