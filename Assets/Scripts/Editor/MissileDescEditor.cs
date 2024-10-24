using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(MissileDesc))]
	public class MissileDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Filter { get; set; }
		private SerializedProperty Range { get; set; }
		private SerializedProperty Speed { get; set; }
		private SerializedProperty Radius { get; set; }
		private SerializedProperty HitOperation { get; set; }

		private void OnEnable()
		{
			Filter = serializedObject.FindProperty(nameof(MissileDesc.Filter).ToBackingField());
			Range = serializedObject.FindProperty(nameof(MissileDesc.Range).ToBackingField());
			Speed = serializedObject.FindProperty(nameof(MissileDesc.Speed).ToBackingField());
			Radius = serializedObject.FindProperty(nameof(MissileDesc.Radius).ToBackingField());
			HitOperation = serializedObject.FindProperty(nameof(MissileDesc.HitOperation).ToBackingField());
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.PropertyField(Filter);
			EditorGUILayout.PropertyField(Range);
			EditorGUILayout.PropertyField(Speed);
			EditorGUILayout.PropertyField(Radius);
			HitOperation.OperationField(typeof(Unit));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
