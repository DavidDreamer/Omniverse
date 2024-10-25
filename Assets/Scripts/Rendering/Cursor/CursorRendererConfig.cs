using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Configs/Rendering/Cursor")]
	public class CursorRendererConfig : ScriptableObject
	{
		[field: SerializeField]
		public CursorParams Default { get; private set; }

		[field: SerializeField]
		public CursorParams Target { get; private set; }

		[field: SerializeField]
		public CursorParams UnitTarget { get; private set; }
	}
}