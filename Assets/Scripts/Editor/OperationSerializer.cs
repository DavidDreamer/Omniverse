using System;
using System.Linq;
using Dreambox.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	public static class OperationSerializer
	{
		public static void OptionalOperationField(this SerializedProperty operation, Type inputType)
		{
			bool enabled = EditorGUILayout.ToggleLeft(operation.displayName, operation.managedReferenceValue != null);

			if (enabled)
			{
				operation.OperationField(inputType);
			}
			else
			{
				operation.managedReferenceValue = null;
			}
		}

		public static void OperationField(this SerializedProperty operation, Type inputType)
		{
			Type[] types = new[]
			{
				typeof(Unit),
				typeof(ResourceSource),
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
			Type operationType = typeof(Operation<,>).MakeGenericType(inputType, genericParameterType);

			if (operation.managedReferenceValue == null || operation.managedReferenceValue.GetType() != operationType)
			{
				operation.managedReferenceValue = Activator.CreateInstance(operationType);
			}

			SerializedProperty targetConverter = operation.FindPropertyRelative("TargetConverter".ToBackingField());
			Type targetConverterType = typeof(ITargetConverter<,>).MakeGenericType(inputType, genericParameterType);
			targetConverter.VersatileField(targetConverterType);

			operation.OperationFieldInternal(genericParameterType);
		}

		private static void OperationFieldInternal(this SerializedProperty operation, Type targetType)
		{
			SerializedProperty actions = operation.FindPropertyRelative("Actions".ToBackingField());
			Type actionType = typeof(IAction<>).MakeGenericType(targetType);

			EditorGUILayout.LabelField("Actions");

			using (new EditorGUI.IndentLevelScope(1))
			{
				DrawActions(actions, actionType);
			}

			void DrawActions(SerializedProperty serializedProperty, Type type)
			{
				for (int i = 0; i < serializedProperty.arraySize; ++i)
				{
					var action = serializedProperty.GetArrayElementAtIndex(i);
					action.VersatileField(type);
				}

				serializedProperty.DrawArrayToolbar();
			}
		}
	}
}
