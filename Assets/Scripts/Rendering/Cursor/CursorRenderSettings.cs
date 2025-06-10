using UnityEngine;

namespace Omniverse.Rendering
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Rendering/Cursor")]
	public class CursorRenderSettings : ScriptableObject
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