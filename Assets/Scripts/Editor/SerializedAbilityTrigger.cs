using System;
using Dreambox.Core.Editor;
using Omniverse.Abilities;
using UnityEditor;

namespace Omniverse.Editor
{
	public class SerializedAbilityTrigger
	{
		public SerializedProperty SerializedProperty { get; }

		public Type TargetType { get; }

		public SerializedOperation Operation { get; }

		public SerializedAbilityTrigger(SerializedProperty serializedProperty, Type targetType)
		{
			SerializedProperty = serializedProperty;
			TargetType = targetType;

			SerializedProperty operationsProperty = SerializedProperty.FindPropertyRelative("Operations".ToBackingField());
			Operation = new SerializedOperation(operationsProperty, targetType);
		}

		public void Draw()
		{
			SerializedProperty.VersatileField(typeof(IAbilityTrigger), false);
			Operation.Draw();
		}
	}
}
