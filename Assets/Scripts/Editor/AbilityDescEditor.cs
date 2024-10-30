using System;
using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;
using UnityEditor.Rendering;
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

		private SerializedOperation ActiveOperation { get; set; }

		private SerializedProperty Triggers { get; set; }
		private SerializedAbilityTriggers TriggersList { get; set; }

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
			ActiveOperation = new(serializedObject.FindProperty(nameof(AbilityDesc.ActiveOperation).ToBackingField()), typeof(NoneTarget));

			Triggers = serializedObject.FindProperty(nameof(AbilityDesc.Triggers).ToBackingField());
			TriggersList = new(Triggers, typeof(Unit));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			DrawMeta();
			DrawTarget();
			DrawSection(Cost);
			DrawCasting();
			DrawSection(Cooldown);
			DrawActiveOperation();
			DrawTriggers();

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
				Target.VersatileField(typeof(ITarget), true);
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

		private void DrawActiveOperation()
		{
			if (DrawSectionHeader(ActiveOperation.SerializedProperty))
			{
				return;
			}

			Type targetType = GetOperationTagetType();
			if (ActiveOperation is null || ActiveOperation.TargetType != targetType)
			{
				ActiveOperation = new(ActiveOperation.SerializedProperty, targetType);
			}

			ActiveOperation.Draw();
			
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

		private void DrawTriggers()
		{
			if (DrawSectionHeader(Triggers))
			{
				TriggersList.Draw();
			}
		}

		private bool DrawSectionHeader(SerializedProperty serializedProperty)
		{
			CoreEditorUtils.DrawSplitter();

			serializedProperty.isExpanded = CoreEditorUtils.DrawHeaderFoldout(serializedProperty.displayName, serializedProperty.isExpanded);

			return serializedProperty.isExpanded;
		}
	}
}
