using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EffectDesc))]
	public class EffectDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Icon { get; set; }
		private SerializedProperty Prefab { get; set; }
		private SerializedProperty IsPositive { get; set; }
		private SerializedProperty Time { get; set; }
		private SerializedProperty UnitStatus { get; set; }
		private SerializedProperty PropertyModifiers { get; set; }
		private SerializedOperation OnAppliedOperation { get; set; }
		private SerializedOperation OnTickOperation { get; set; }
		private SerializedOperation OnRemovedOperation { get; set; }

		private void OnEnable()
		{
			Icon = serializedObject.FindProperty(nameof(EffectDesc.Icon).ToBackingField());
			Prefab = serializedObject.FindProperty(nameof(EffectDesc.Prefab).ToBackingField());
			IsPositive = serializedObject.FindProperty(nameof(EffectDesc.IsPositive).ToBackingField());
			Time = serializedObject.FindProperty(nameof(EffectDesc.Duration).ToBackingField());
			UnitStatus = serializedObject.FindProperty(nameof(EffectDesc.UnitStatus).ToBackingField());
			PropertyModifiers = serializedObject.FindProperty(nameof(EffectDesc.PropertyModifiers).ToBackingField());
			OnAppliedOperation = new(serializedObject.FindProperty(nameof(EffectDesc.OnAppliedOperation).ToBackingField()), typeof(None));
			OnTickOperation = new(serializedObject.FindProperty(nameof(EffectDesc.OnTickOperation).ToBackingField()), typeof(None));
			OnRemovedOperation = new(serializedObject.FindProperty(nameof(EffectDesc.OnRemovedOperation).ToBackingField()), typeof(None));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.PropertyField(Icon);
			EditorGUILayout.PropertyField(Prefab);
			EditorGUILayout.PropertyField(IsPositive);
			EditorGUILayout.PropertyField(Time);
			EditorGUILayout.PropertyField(UnitStatus);
			EditorGUILayout.PropertyField(PropertyModifiers);
			OnAppliedOperation.Draw();
			OnTickOperation.Draw();
			OnRemovedOperation.Draw();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
