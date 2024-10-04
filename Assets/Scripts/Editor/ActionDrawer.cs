using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	[CustomEditor(typeof(Actions.Action), true)]
	public class ActionDrawer : UnityEditor.Editor
	{
		private SerializedProperty Then { get; set; }

		private void OnEnable()
		{
			Then = serializedObject.FindProperty(nameof(Actions.Action.Then).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			var iterator = serializedObject.GetIterator();

			//Skip script link and 'Then'
			iterator.NextVisible(true);
			iterator.NextVisible(true);

			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}

			EditorGUILayout.PropertyField(Then);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
