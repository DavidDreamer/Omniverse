using System;
using System.Linq;
using Dreambox.Core.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Omniverse.Editor
{
	public class SerializedActions
	{
		private SerializedProperty SerializedProperty { get; }

		private ReorderableList ReorderableList { get; }

		public Type TargetType { get; }

		public SerializedActions(SerializedProperty serializedProperty, Type targetType)
		{
			SerializedProperty = serializedProperty;
			TargetType = targetType;

			ReorderableList = new ReorderableList(serializedProperty.serializedObject, serializedProperty, true, true, true, true)
			{
				drawHeaderCallback = DrawHeader,
				drawElementCallback = DrawElement,
				multiSelect = false
			};
		}

		private void DrawHeader(Rect rect) => EditorGUI.LabelField(rect, SerializedProperty.displayName);

		public void Draw()
		{
			ReorderableList.DoLayoutList();

			if (ReorderableList.selectedIndices.Count > 0)
			{
				int selectedActionIndex = ReorderableList.selectedIndices.First();
				SerializedProperty selectedAction = SerializedProperty.GetArrayElementAtIndex(selectedActionIndex);
				selectedAction.DrawChildren();
			}
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty action = SerializedProperty.GetArrayElementAtIndex(index);
			Type actionType = typeof(IAction<>).MakeGenericType(TargetType);
			action.VersatileField(rect, actionType);
		}
	}
}
