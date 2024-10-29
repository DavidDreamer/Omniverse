using System;
using System.Collections.Generic;
using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	public class SerializedAbilityTriggers
	{
		public SerializedProperty SerializedProperty { get; }

		public Type TargetType { get; }

		private List<SerializedAbilityTrigger> Triggers { get; set; } = new();

		public SerializedAbilityTriggers(SerializedProperty serializedProperty, Type targetType)
		{
			SerializedProperty = serializedProperty;
			TargetType = targetType;
		}

		public void Draw()
		{
			if (Triggers.Count != SerializedProperty.arraySize)
			{
				Triggers.Clear();

				for (int i = 0; i < SerializedProperty.arraySize; i++)
				{
					SerializedAbilityTrigger serializedAbilityTrigger = new(SerializedProperty.GetArrayElementAtIndex(i), TargetType);
					Triggers.Add(serializedAbilityTrigger);
				}
			}

			for (int i = 0; i < Triggers.Count; i++)
			{
				SerializedAbilityTrigger serializedAbilityTrigger = Triggers[i];
				serializedAbilityTrigger.Draw();
			}

			SerializedProperty.DrawArrayToolbar();
		}
	}
}
