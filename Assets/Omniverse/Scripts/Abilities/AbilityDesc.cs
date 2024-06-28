using System.Collections.Generic;
using Dreambox.Core;
using Omniverse.Actions;
using UnityEngine;

namespace Omniverse.Abilities
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Ability")]
	public class AbilityDesc: ScriptableObject
	{
		[field: SerializeField]
		public Presentation Presentation { get; private set; }

		[field: SerializeField]
		public Cast Cast { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(ITarget))]
		public ITarget Target { get; private set; }

		[field: SerializeField]
		public CooldownDesc Cooldown { get; private set; }

		[field: SerializeField]
		public List<CostDesc> Cost { get; private set; }

		[field: SerializeReference]
		[field: Versatile(typeof(IActionDesc))]
		public IActionDesc[] Actions { get; private set; }
	}
}
