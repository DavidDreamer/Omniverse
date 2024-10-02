using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;
using UnityEngine;

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
		private SerializedProperty Operation { get; set; }

		private void OnEnable()
		{
			Meta = serializedObject.FindProperty(nameof(Meta).ToBackingField());
			Cast = serializedObject.FindProperty(nameof(Cast).ToBackingField());
			Target = serializedObject.FindProperty(nameof(Target).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Operation = serializedObject.FindProperty(nameof(Operation).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			DrawSection(Meta);
			DrawSection(Cast);
			DrawSection(Target);
			DrawSection(Cooldown);
			DrawSection(Cost);
			DrawAction(Operation);

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawSection(SerializedProperty serializedProperty)
		{
			if (GUILayout.Button(serializedProperty.displayName))
			{
				serializedProperty.isExpanded = !serializedProperty.isExpanded;
			}

			if (serializedProperty.isExpanded)
			{
				serializedProperty.DrawChildren();
			}
		}

		private void DrawAction(SerializedProperty serializedProperty)
		{
			if (GUILayout.Button(serializedProperty.displayName))
			{
				serializedProperty.isExpanded = !serializedProperty.isExpanded;
			}

			if (serializedProperty.isExpanded)
			{
				EditorGUILayout.PropertyField(serializedProperty);
			}
		}
	}
}
