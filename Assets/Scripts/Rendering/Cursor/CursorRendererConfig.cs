using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Configs/Rendering/Cursor")]
	public class CursorRendererConfig : ScriptableObject
	{
		[field: SerializeField]
		public CursorParams Default { get; private set; }

		[field: SerializeField]
		public CursorParams HoverAlly { get; private set; }

		[field: SerializeField]
		public CursorParams HoverEnemy { get; private set; }

		[field: SerializeField]
		public CursorParams TargetDefault { get; private set; }

		[field: SerializeField]
		public CursorParams TargetInvalid { get; private set; }

		[field: SerializeField]
		public CursorParams TargetUnit { get; private set; }
	}
}