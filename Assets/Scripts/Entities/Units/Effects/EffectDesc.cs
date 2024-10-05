using System.Collections.Generic;
using Omniverse.Actions;
using UnityEngine;

namespace Omniverse.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Effect")]
	public class EffectDesc : ScriptableObject
	{
		[field: SerializeField]
		public Sprite Icon { get; private set; }

		[field: SerializeField]
		public GameObject Prefab { get; private set; }

		[field: SerializeField]
		public bool IsPositive { get; private set; }

		[field: SerializeField]
		public float Time { get; private set; }

		[field: SerializeField]
		public UnitStatus UnitStatus { get; private set; }

		[field: SerializeField]
		public List<PropertyModifierDesc> PropertyModifiers { get; private set; }

		[field: SerializeField]
		[field: OperationPicker]
		public Operation OnRemovedOperation { get; set; }
	}
}
