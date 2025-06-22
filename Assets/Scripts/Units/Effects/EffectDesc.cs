using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
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
		public float Duration { get; private set; }

		[field: SerializeField]
		public UnitStatus UnitStatus { get; private set; }

		[field: SerializeField]
		public List<PropertyModifierDesc> PropertyModifiers { get; private set; }

		[field: SerializeReference]
		public IOperation<None> OnAppliedOperation { get; set; }

		[field: SerializeReference]
		public IOperation<None> OnTickOperation { get; set; }

		[field: SerializeReference]
		public IOperation<None> OnRemovedOperation { get; set; }
	}
}
