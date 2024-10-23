using System;
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
		private SerializedProperty Meta { get; set; }
		private SerializedProperty MetaName { get; set; }
		private SerializedProperty MetaDescription { get; set; }
		private SerializedProperty MetaIcon { get; set; }

		private SerializedProperty Target { get; set; }
		private SerializedProperty TargetType { get; set; }
		private SerializedProperty TargetFilter { get; set; }
		private SerializedProperty TargetResourceSources { get; set; }

		private SerializedProperty Casting { get; set; }

		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }

		private void OnEnable()
		{
			Meta = serializedObject.FindProperty(nameof(AbilityDesc.Meta).ToBackingField());
			MetaName = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Name).ToBackingField());
			MetaDescription = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Description).ToBackingField());
			MetaIcon = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Icon).ToBackingField());

			Target = serializedObject.FindProperty(nameof(AbilityDesc.Target).ToBackingField());
			TargetType = Target.FindPropertyRelative(nameof(AbilityDesc.Target.Type).ToBackingField());
			TargetFilter = Target.FindPropertyRelative(nameof(AbilityDesc.Target.Filter).ToBackingField());
			TargetResourceSources = Target.FindPropertyRelative(nameof(AbilityDesc.Target.ResourceSources).ToBackingField());

			Casting = serializedObject.FindProperty(nameof(AbilityDesc.Casting).ToBackingField());

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
			DrawOperation();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawMeta()
		{
			if (DrawSectionHeader(Meta))
			{
				EditorGUILayout.PropertyField(MetaName);
				EditorGUILayout.PropertyField(MetaDescription);
				MetaIcon.DrawIcon();
			}
		}

		private void DrawTarget()
		{
			if (DrawSectionHeader(Target))
			{
				EditorGUILayout.PropertyField(TargetType);

				var targetType = (TargetType)TargetType.enumValueFlag;
				if (targetType is Abilities.TargetType.None)
				{
					return;
				}

				if (targetType.HasFlag(Abilities.TargetType.Unit))
				{
					EditorGUILayout.PropertyField(TargetFilter);
				}

				if (targetType.HasFlag(Abilities.TargetType.ResourceSource))
				{
					EditorGUILayout.PropertyField(TargetResourceSources);
				}
			}
		}

		private void DrawCasting()
		{
			DrawSection(Casting);
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
			SerializedProperty operation = serializedObject.FindProperty(nameof(AbilityDesc.Action).ToBackingField());
			DrawSectionHeader(operation);

			if (operation.isExpanded)
			{
				var actions = operation.FindPropertyRelative(nameof(AbilityDesc.Action.Actions).ToBackingField());

				for (int i = 0; i < actions.arraySize; ++i)
				{
					var action = actions.GetArrayElementAtIndex(i);
					EditorGUILayout.PropertyField(action);
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Add"))
					{
						actions.arraySize++;
					}

					if (GUILayout.Button("Remove"))
					{
						var action = actions.GetArrayElementAtIndex(actions.arraySize - 1);
						if (action.objectReferenceValue != null)
						{
							AssetDatabase.RemoveObjectFromAsset(action.objectReferenceValue);
						}
						actions.DeleteArrayElementAtIndex(actions.arraySize - 1);
					}
				}
			}
		}

		private void DrawOperation()
		{
			SerializedProperty operation = serializedObject.FindProperty(nameof(AbilityDesc.Operation).ToBackingField());
			DrawSectionHeader(operation);

			if (!operation.isExpanded)
			{
				return;
			}

			operation.OperationField();
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
