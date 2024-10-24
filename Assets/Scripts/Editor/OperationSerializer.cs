using System;
using System.Linq;
using Dreambox.Core.Editor;
using Omniverse.Abilities;
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

		public static void OperationField(this SerializedProperty operation, Type targetType)
		{
			Type[] types = new[]
			{
				typeof(Unit),
				typeof(Vector3)
			};

			GUIContent[] selectedOptions = types.Select(type => new GUIContent(type.Name)).ToArray();

			int selectedIndex = 0;
			if (operation.managedReferenceValue is null)
			{
				selectedIndex = 0;
			}
			else
			{
				try
				{
					Type currentType = operation.managedReferenceValue.GetType().GetGenericArguments()[1];
					selectedIndex = types.ToList().IndexOf(currentType);
				}
				catch
				{
					selectedIndex = 0;
				}

			}

			GUIContent label = new("Target Type");
			selectedIndex = EditorGUILayout.Popup(label, selectedIndex, selectedOptions);

			Type genericParameterType = types[selectedIndex];
			Type operationType = typeof(Operation<,>).MakeGenericType(targetType, genericParameterType);

			if (operation.managedReferenceValue == null || operation.managedReferenceValue.GetType() != operationType)
			{
				operation.managedReferenceValue = Activator.CreateInstance(operationType);
			}

			SerializedProperty targetConverter = operation.FindPropertyRelative("TargetConverter".ToBackingField());
			Type targetConverterType = typeof(ITargetConverter<,>).MakeGenericType(targetType, genericParameterType);
			targetConverter.DrawVersatile(targetConverterType);

			operation.OperationFieldInternal(genericParameterType);
		}

		public static void OperationField(this SerializedProperty operation)
		{
			Type[] types = new[]
			{
				typeof(Unit),
				typeof(Vector3)
			};

			GUIContent[] selectedOptions = types.Select(type => new GUIContent(type.Name)).ToArray();

			int selectedIndex = 0;
			if (operation.managedReferenceValue is null)
			{
				selectedIndex = 0;
			}
			else
			{
				Type currentType = operation.managedReferenceValue.GetType().GetGenericArguments()[0];
				selectedIndex = types.ToList().IndexOf(currentType);
			}

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

			operation.OperationFieldInternal(genericParameterType);
		}

		private static void OperationFieldInternal(this SerializedProperty operation, Type targetType)
		{
			SerializedProperty actions = operation.FindPropertyRelative("Actions".ToBackingField());
			Type actionType = typeof(IAction<>).MakeGenericType(targetType);
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
