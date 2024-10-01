using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;

namespace Omniverse.Editor
{
	[CustomEditor(typeof(AbilityDesc))]
	public class AbilityDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Meta { get; set; }
		private SerializedProperty Cast { get; set; }
		private SerializedProperty Target { get; set; }
		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }
		private SerializedProperty Action { get; set; }

		private void OnEnable()
		{
			Meta = serializedObject.FindProperty(nameof(Meta).ToBackingField());
			Cast = serializedObject.FindProperty(nameof(Cast).ToBackingField());
			Target = serializedObject.FindProperty(nameof(Target).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Action = serializedObject.FindProperty(nameof(Action).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.PropertyField(Meta);
			EditorGUILayout.PropertyField(Cast);
			EditorGUILayout.PropertyField(Target);
			EditorGUILayout.PropertyField(Cooldown);
			EditorGUILayout.PropertyField(Cost);
			EditorGUILayout.PropertyField(Action);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
