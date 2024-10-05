using UnityEditor;

namespace Omniverse.Editor
{
	[CustomEditor(typeof(Operation), true)]
	public class OperationDrawer : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			var iterator = serializedObject.GetIterator();

			//Skip script link
			iterator.NextVisible(true);

			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
