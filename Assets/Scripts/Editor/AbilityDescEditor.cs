using System;
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
		private SerializedProperty MetaName { get; set; }
		private SerializedProperty MetaDescription { get; set; }
		private SerializedProperty MetaIcon { get; set; }

		private SerializedProperty Target { get; set; }
		private SerializedProperty Casting { get; set; }
		private SerializedProperty Cooldown { get; set; }
		private SerializedProperty Cost { get; set; }
		private SerializedProperty Operations { get; set; }

		private void OnEnable()
		{
			Meta = serializedObject.FindProperty(nameof(AbilityDesc.Meta).ToBackingField());
			MetaName = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Name).ToBackingField());
			MetaDescription = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Description).ToBackingField());
			MetaIcon = Meta.FindPropertyRelative(nameof(AbilityDesc.Meta.Icon).ToBackingField());

			Target = serializedObject.FindProperty(nameof(AbilityDesc.Target).ToBackingField());
			Casting = serializedObject.FindProperty(nameof(AbilityDesc.Casting).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Operations = serializedObject.FindProperty(nameof(AbilityDesc.Operations).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			DrawMeta();
			DrawTarget();
			DrawSection(Cost);
			DrawCasting();
			DrawSection(Cooldown);
			DrawOperations();

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
				Target.DrawVersatile(typeof(ITarget), true);
			}

			void DrawTargetOperations(Type type)
			{
				SerializedProperty operations = Target.FindPropertyRelative("Operations".ToBackingField());

				for (int i = 0; i < operations.arraySize; ++i)
				{
					SerializedProperty operation = operations.GetArrayElementAtIndex(i);
					operation.OperationField(type);
				}

				operations.DrawArrayToolbar();
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

		private void DrawOperations()
		{
			if (DrawSectionHeader(Operations))
			{
				return;
			}
	
			Type targetType = GetOperationTagetType();

			for (int i = 0; i < Operations.arraySize; ++i)
			{
				SerializedProperty op = Operations.GetArrayElementAtIndex(i);
				op.OperationField(targetType);
			}

			Operations.DrawArrayToolbar();

			Type GetOperationTagetType()
			{
				switch (Target.managedReferenceValue)
				{
					case NoneTarget:
						return typeof(None);
					case UnitTarget:
						return typeof(Unit);
					case ResourceSourceTarget:
						return typeof(ResourceSource);
					case VectorTarget:
						return typeof(Vector3);
					default: throw new Exception();
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
