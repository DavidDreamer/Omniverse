using System;
using System.Linq;
using Dreambox.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Omniverse.Editor
{
	public class SerializedOperation
	{
		private static Type[] TargetTypes { get; } = new[]
		{
			typeof(Unit),
			typeof(ResourceSource),
			typeof(Vector3)
		};

		public SerializedProperty SerializedProperty { get; }

		public Type TargetType { get; }

		private Type ProcessedTargetType { get; set; }

		private SerializedActions SerializedActions { get; set; }

		public SerializedOperation(SerializedProperty serializedProperty, Type targetType)
		{
			SerializedProperty = serializedProperty;
			TargetType = targetType;

			int index = GetProcessedTargetTypeIndex();
			SetProcessTargetType(index);

			SerializedProperty actionsProperty = SerializedProperty.FindPropertyRelative("Actions".ToBackingField());
			SerializedActions = new SerializedActions(actionsProperty, ProcessedTargetType);
		}

		public void Draw()
		{
			bool hasValue = SerializedProperty.managedReferenceValue is not null;
			bool shouldHaveValue = EditorGUILayout.ToggleLeft(SerializedProperty.displayName, hasValue);

			if (!shouldHaveValue)
			{
				SerializedProperty.managedReferenceValue = null;
				return;
			}

			GUIContent[] selectedOptions = TargetTypes.Select(type => new GUIContent(type.Name)).ToArray();

			int selectedIndex = GetProcessedTargetTypeIndex();
			GUIContent label = new("Target Type");
			selectedIndex = EditorGUILayout.Popup(label, selectedIndex, selectedOptions);

			SetProcessTargetType(selectedIndex);

			SerializedProperty targetConverter = SerializedProperty.FindPropertyRelative("TargetConverter".ToBackingField());
			Type targetConverterType = typeof(ITargetConverter<,>).MakeGenericType(TargetType, ProcessedTargetType);
			targetConverter.VersatileField(targetConverterType);

			if (SerializedActions.TargetType != ProcessedTargetType)
			{
				SerializedProperty actionsProperty = SerializedProperty.FindPropertyRelative("Actions".ToBackingField());
				SerializedActions = new SerializedActions(actionsProperty, ProcessedTargetType);
			}

			SerializedActions.Draw();
		}

		private int GetProcessedTargetTypeIndex()
		{
			if (SerializedProperty.managedReferenceValue is null)
			{
				return 0;
			}

			Type currentType = SerializedProperty.managedReferenceValue.GetType().GetGenericArguments()[1];
			int cachedIndex = TargetTypes.ToList().IndexOf(currentType);

			return cachedIndex == -1 ? 0 : cachedIndex;
		}

		private void SetProcessTargetType(int index)
		{
			ProcessedTargetType = TargetTypes[index];

			Type operationType = typeof(Operation<,>).MakeGenericType(TargetType, ProcessedTargetType);

			if (SerializedProperty.managedReferenceValue == null || SerializedProperty.managedReferenceValue.GetType() != operationType)
			{
				SerializedProperty.managedReferenceValue = Activator.CreateInstance(operationType);
			}
		}
	}
}
