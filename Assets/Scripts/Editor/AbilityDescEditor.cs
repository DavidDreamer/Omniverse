using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Dreambox.Core.Editor;
using NUnit.Framework;
using Omniverse.Abilities;
using Omniverse.Actions;
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
		private SerializedProperty Action { get; set; }

		private List<Type> ActionTypes { get; set; }
		private string[] ActionNames { get; set; }

		private void OnEnable()
		{
			Meta = serializedObject.FindProperty(nameof(Meta).ToBackingField());
			Cast = serializedObject.FindProperty(nameof(Cast).ToBackingField());
			Target = serializedObject.FindProperty(nameof(Target).ToBackingField());
			Cooldown = serializedObject.FindProperty(nameof(Cooldown).ToBackingField());
			Cost = serializedObject.FindProperty(nameof(Cost).ToBackingField());
			Action = serializedObject.FindProperty(nameof(Action).ToBackingField());

			ActionTypes = TypeUtils.GetInheritedTypes(typeof(Actions.Action)).ToList();
			var actionNames = ActionTypes.Select(type => type.Name).ToList();

			ActionTypes.Insert(0, null);

			actionNames.Insert(0, "None");
			ActionNames = actionNames.ToArray();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();
			
			DrawSection(Meta);
			DrawSection(Cast);
			DrawSection(Target);
			DrawSection(Cooldown);
			DrawSection(Cost);
			DrawAction(Action);

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
				ValidateActionType(serializedProperty);

				while (serializedProperty.objectReferenceValue != null)
				{
					var editor = CreateEditor(serializedProperty.objectReferenceValue);
					editor.OnInspectorGUI();

					serializedProperty = editor.serializedObject.FindProperty("Next".ToBackingField());

					ValidateActionType(serializedProperty);

					editor.serializedObject.ApplyModifiedProperties();
			
				}
			}
		}

		private void ValidateActionType(SerializedProperty serializedProperty)
		{
			UnityEngine.Object objectReferenceValue = serializedProperty.objectReferenceValue;
			
			var typeIndex = objectReferenceValue == null ? 0 : ActionTypes.IndexOf(serializedProperty.objectReferenceValue.GetType());

			int index = EditorGUILayout.Popup(typeIndex, ActionNames);

			var type = ActionTypes[index];

			if (type == null)
			{
				if (objectReferenceValue != null)
				{
					DeleteAsset();
				}
			}
			else
			{
				if (objectReferenceValue == null)
				{
					CreateAsset();
				}
				else
				{
					if (objectReferenceValue.GetType() != type)
					{
						DeleteAsset();
						CreateAsset();
					}
				}
			}

			void CreateAsset()
			{
				var instance = CreateInstance(type);
				instance.name = type.Name;
				AssetDatabase.AddObjectToAsset(instance, serializedObject.targetObject);
				AssetDatabase.SaveAssets();
				serializedProperty.objectReferenceValue = instance;
			}

			void DeleteAsset()
			{
				AssetDatabase.RemoveObjectFromAsset(objectReferenceValue);
				AssetDatabase.SaveAssets();
				DestroyImmediate(objectReferenceValue);
			}
		}
	}
}
