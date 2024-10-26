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
		private SerializedProperty OnRemovedOperation { get; set; }

		private void OnEnable()
		{
			Icon = serializedObject.FindProperty(nameof(EffectDesc.Icon).ToBackingField());
			Prefab = serializedObject.FindProperty(nameof(EffectDesc.Prefab).ToBackingField());
			IsPositive = serializedObject.FindProperty(nameof(EffectDesc.IsPositive).ToBackingField());
			Time = serializedObject.FindProperty(nameof(EffectDesc.Time).ToBackingField());
			UnitStatus = serializedObject.FindProperty(nameof(EffectDesc.UnitStatus).ToBackingField());
			PropertyModifiers = serializedObject.FindProperty(nameof(EffectDesc.PropertyModifiers).ToBackingField());
			OnRemovedOperation = serializedObject.FindProperty(nameof(EffectDesc.OnRemovedOperation).ToBackingField());
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
			OnRemovedOperation.OptionalOperationField(typeof(None));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
