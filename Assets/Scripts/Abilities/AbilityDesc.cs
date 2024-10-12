using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Abilities
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Ability")]
	public class AbilityDesc : ScriptableObject
	{
		[field: SerializeField]
		public Meta Meta { get; private set; }

		[field: SerializeField]
		public TargetDesc Target { get; private set; }

		[field: SerializeField]
		public List<CostDesc> Cost { get; private set; }

		[field: SerializeField]
		public CastingDesc Casting { get; private set; }

		[field: SerializeField]
		public CooldownDesc Cooldown { get; private set; }

		[field: SerializeField]
		public MultiAction Action { get; private set; }
	}
}
