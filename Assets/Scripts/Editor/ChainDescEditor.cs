using Dreambox.Core.Editor;
using UnityEditor;

namespace Omniverse.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ChainDesc))]
	public class ChainDescEditor : UnityEditor.Editor
	{
		private SerializedProperty Filter { get; set; }
		private SerializedProperty MaxTargets { get; set; }
		private SerializedProperty BounceRange { get; set; }
		private SerializedProperty BounceInterval { get; set; }
		private SerializedOperation Operation { get; set; }

		private void OnEnable()
		{
			Filter = serializedObject.FindProperty(nameof(ChainDesc.Filter).ToBackingField());
			MaxTargets = serializedObject.FindProperty(nameof(ChainDesc.MaxTargets).ToBackingField());
			BounceRange = serializedObject.FindProperty(nameof(ChainDesc.BounceRange).ToBackingField());
			BounceInterval = serializedObject.FindProperty(nameof(ChainDesc.BounceInterval).ToBackingField());
			Operation = new(serializedObject.FindProperty(nameof(ChainDesc.Operation).ToBackingField()), typeof(UnitObsolete));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.PropertyField(Filter);
			EditorGUILayout.PropertyField(MaxTargets);
			EditorGUILayout.PropertyField(BounceRange);
			EditorGUILayout.PropertyField(BounceInterval);
			Operation.Draw();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
