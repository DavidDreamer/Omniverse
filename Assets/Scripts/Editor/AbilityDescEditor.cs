using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	[CustomEditor(typeof(AbilityDesc))]
	public class AbilityDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Cast { get; set; }
		private SerializedProperty Target { get; set; }
		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }
		private SerializedProperty Operation { get; set; }

		private void OnEnable()
		{
			Cast = serializedObject.FindProperty(nameof(Cast).ToBackingField());
			Target = serializedObject.FindProperty(nameof(Target).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Operation = serializedObject.FindProperty(nameof(Operation).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			DrawMeta();
			DrawSection(Cast);
			DrawSection(Target);
			DrawSection(Cooldown);
			DrawSection(Cost);
			DrawAction(Operation);

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawMeta()
		{
			SerializedProperty meta = serializedObject.FindProperty(nameof(Meta).ToBackingField());

			if (DrawSectionHeader(meta))
			{
				SerializedProperty icon = meta.FindPropertyRelative(nameof(Meta.Icon).ToBackingField());
				icon.DrawIcon();
			}
		}

		private void DrawSection(SerializedProperty serializedProperty)
		{
			DrawSectionHeader(serializedProperty);

			if (serializedProperty.isExpanded)
			{
				serializedProperty.DrawChildren();
			}
		}

		private void DrawAction(SerializedProperty serializedProperty)
		{
			DrawSectionHeader(serializedProperty);

			if (serializedProperty.isExpanded)
			{
				EditorGUILayout.PropertyField(serializedProperty);
			}
		}

		private bool DrawSectionHeader(SerializedProperty serializedProperty)
		{
			if (GUILayout.Button(serializedProperty.displayName))
			{
				serializedProperty.isExpanded = !serializedProperty.isExpanded;
			}

			return serializedProperty.isExpanded;
		}
	}
}
