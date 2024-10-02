using System;
using System.Collections.Generic;
using Dreambox.Core.Editor;
using System.Linq;
using Omniverse.Actions;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	[CustomPropertyDrawer(typeof(ActionPickerAttribute))]
	public class ActionPickerAttributeDrawer : PropertyDrawer
	{
		private static List<Type> ActionTypes { get; set; }

		private static GUIContent[] ActionNames { get; set; }

		static ActionPickerAttributeDrawer()
		{
			ActionTypes = TypeUtils.GetInheritedTypes(typeof(Actions.Action)).ToList();
			var actionNames = ActionTypes.Select(type => type.Name).ToList();

			ActionTypes.Insert(0, null);

			actionNames.Insert(0, "None");
			ActionNames = actionNames.Select(name => new GUIContent(name)).ToArray();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ValidateType(position, property, label);
		}

		private void ValidateType(Rect position, SerializedProperty property, GUIContent label)
		{
			UnityEngine.Object objectReferenceValue = property.objectReferenceValue;

			var typeIndex = objectReferenceValue == null ? 0 : ActionTypes.IndexOf(objectReferenceValue.GetType());

			int index = EditorGUI.Popup(position, label, typeIndex, ActionNames);

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
				var instance = ScriptableObject.CreateInstance(type);
				instance.name = type.Name;
				AssetDatabase.AddObjectToAsset(instance, property.serializedObject.targetObject);
				AssetDatabase.SaveAssets();
				property.objectReferenceValue = instance;
			}

			void DeleteAsset()
			{
				AssetDatabase.RemoveObjectFromAsset(objectReferenceValue);
				AssetDatabase.SaveAssets();
				UnityEngine.Object.DestroyImmediate(objectReferenceValue);
			}
		}
	}
}
