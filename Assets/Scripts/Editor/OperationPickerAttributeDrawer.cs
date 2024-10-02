using System;
using System.Collections.Generic;
using Dreambox.Core.Editor;
using System.Linq;
using Omniverse.Actions;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	[CustomPropertyDrawer(typeof(OperationPickerAttribute))]
	public class OperationPickerAttributeDrawer : PropertyDrawer
	{
		private static List<Type> OperationTypes { get; set; }

		private static GUIContent[] OperationNames { get; set; }

		static OperationPickerAttributeDrawer()
		{
			OperationTypes = TypeUtils.GetInheritedTypes(typeof(Operation)).ToList();
			var operationNames = OperationTypes.Select(type => type.Name).ToList();

			OperationTypes.Insert(0, null);

			operationNames.Insert(0, "None");
			OperationNames = operationNames.Select(name => new GUIContent(name)).ToArray();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ValidateType(position, property, label);
		}

		private void ValidateType(Rect position, SerializedProperty property, GUIContent label)
		{
			UnityEngine.Object objectReferenceValue = property.objectReferenceValue;

			var typeIndex = objectReferenceValue == null ? 0 : OperationTypes.IndexOf(objectReferenceValue.GetType());

			int index = EditorGUI.Popup(position, label, typeIndex, OperationNames);

			var type = OperationTypes[index];

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
