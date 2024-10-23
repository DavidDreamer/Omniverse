using System;
using System.Linq;
using Dreambox.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	public static class OperationSerializer
	{
		public static void OptionalOperationField(this SerializedProperty operation)
		{
			bool enabled = EditorGUILayout.ToggleLeft(operation.displayName, operation.managedReferenceValue != null);

			if (enabled)
			{
				operation.OperationField();
			}
			else
			{
				operation.managedReferenceValue = null;
			}
		}

		public static void OperationField(this SerializedProperty operation)
		{
			Type[] types = new[]
			{
				typeof(Unit),
				typeof(Vector3)
			};

			GUIContent[] selectedOptions = types.Select(type => new GUIContent(type.Name)).ToArray();

			int selectedIndex = operation.managedReferenceValue is null ? 0 : types.ToList().IndexOf(operation.managedReferenceValue.GetType().GetGenericArguments()[0]);

			GUIContent label = new("Target Type");
			selectedIndex = EditorGUILayout.Popup(label, selectedIndex, selectedOptions);

			Type genericParameterType = types[selectedIndex];
			Type operationType = typeof(Operation<>).MakeGenericType(genericParameterType);

			if (operation.managedReferenceValue == null || operation.managedReferenceValue.GetType() != operationType)
			{
				operation.managedReferenceValue = Activator.CreateInstance(operationType);
			}

			SerializedProperty targetProvider = operation.FindPropertyRelative("TargetProvider".ToBackingField());
			Type targetProviderType = typeof(ITargetProvider<>).MakeGenericType(genericParameterType);
			targetProvider.DrawVersatile(targetProviderType);

			SerializedProperty actions = operation.FindPropertyRelative("Actions".ToBackingField());
			Type actionType = typeof(IAction<>).MakeGenericType(genericParameterType);
			DrawActions(actions, actionType);

			void DrawActions(SerializedProperty serializedProperty, Type type)
			{
				for (int i = 0; i < serializedProperty.arraySize; ++i)
				{
					var action = serializedProperty.GetArrayElementAtIndex(i);
					action.DrawVersatile(type);
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Add"))
					{
						serializedProperty.arraySize++;
					}

					if (GUILayout.Button("Remove"))
					{
						serializedProperty.arraySize--;
					}
				}
			}
		}
	}
}
