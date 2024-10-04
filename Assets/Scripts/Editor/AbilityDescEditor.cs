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
		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }
		private SerializedProperty Operation { get; set; }

		private void OnEnable()
		{
			Cast = serializedObject.FindProperty(nameof(Cast).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Operation = serializedObject.FindProperty(nameof(Operation).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			DrawMeta();
			DrawTarget();
			DrawSection(Cast);
			DrawSection(Cooldown);
			DrawSection(Cost);
			DrawAction(Operation);

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawMeta()
		{
			SerializedProperty meta = serializedObject.FindProperty(nameof(AbilityDesc.Meta).ToBackingField());

			if (DrawSectionHeader(meta))
			{
				SerializedProperty icon = meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Icon).ToBackingField());
				icon.DrawIcon();
			}
		}

		private void DrawTarget()
		{
			SerializedProperty target = serializedObject.FindProperty(nameof(AbilityDesc.Target).ToBackingField());

			if (DrawSectionHeader(target))
			{
				SerializedProperty type = target.FindPropertyRelative(nameof(AbilityDesc.Target.Type).ToBackingField());
				EditorGUILayout.PropertyField(type);

				var targetType = (TargetType)type.enumValueFlag;
				if (targetType is TargetType.None)
				{
					return;
				}

				SerializedProperty range = target.FindPropertyRelative(nameof(AbilityDesc.Target.Range).ToBackingField());
				EditorGUILayout.PropertyField(range);

				if (targetType.HasFlag(TargetType.Unit))
				{
					SerializedProperty filter = target.FindPropertyRelative(nameof(AbilityDesc.Target.Filter).ToBackingField());
					EditorGUILayout.PropertyField(filter);
				}

				if (targetType.HasFlag(TargetType.ResourceSource))
				{
					SerializedProperty resourceSources = target.FindPropertyRelative(nameof(AbilityDesc.Target.ResourceSources).ToBackingField());
					EditorGUILayout.PropertyField(resourceSources);
				}
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
