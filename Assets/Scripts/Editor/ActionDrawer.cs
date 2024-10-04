using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	[CustomEditor(typeof(Actions.Action), true)]
	public class ActionDrawer : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			SerializedProperty then = serializedObject.FindProperty(nameof(Actions.Action.Then).ToBackingField());

			serializedObject.UpdateIfRequiredOrScript();

			var iterator = serializedObject.GetIterator();

			//Skip script link and 'Then'
			iterator.NextVisible(true);
			iterator.NextVisible(true);

			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}

			EditorGUILayout.PropertyField(then);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
