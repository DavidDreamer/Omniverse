using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(AbilityDesc))]
	public class AbilityDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }

		private void OnEnable()
		{
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			DrawMeta();
			DrawTarget();
			DrawSection(Cost);
			DrawCasting();
			DrawSection(Cooldown);
			DrawAction();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawMeta()
		{
			SerializedProperty meta = serializedObject.FindProperty(nameof(AbilityDesc.Meta).ToBackingField());

			if (DrawSectionHeader(meta))
			{
				SerializedProperty name = meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Name).ToBackingField());
				EditorGUILayout.PropertyField(name);

				SerializedProperty description = meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Description).ToBackingField());
				EditorGUILayout.PropertyField(description);

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

		private void DrawCasting()
		{
			SerializedProperty casting = serializedObject.FindProperty(nameof(AbilityDesc.Casting).ToBackingField());
			DrawSection(casting);
		}

		private void DrawSection(SerializedProperty serializedProperty)
		{
			DrawSectionHeader(serializedProperty);

			if (serializedProperty.isExpanded)
			{
				serializedProperty.DrawChildren();
			}
		}

		private void DrawAction()
		{
			SerializedProperty action = serializedObject.FindProperty(nameof(AbilityDesc.Action).ToBackingField());
			DrawSectionHeader(action);

			if (action.isExpanded)
			{
				var actions = action.FindPropertyRelative(nameof(AbilityDesc.Action.Actions).ToBackingField());

				for (int i = 0; i < actions.arraySize; ++i)
				{
					EditorGUILayout.PropertyField(actions.GetArrayElementAtIndex(i));
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Add"))
					{
						actions.arraySize++;
					}
					if (GUILayout.Button("Remove"))
					{
						//TODO asset deletion
						actions.arraySize--;
					}
				}
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
