using UnityEngine;

namespace Omniverse.Abilities
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Ability")]
	public class AbilityDesc : ScriptableObject
	{
		[field: SerializeField]
		public Meta Meta { get; private set; }

		[field: SerializeField]
		public Target Target { get; private set; }

		[field: SerializeField]
		public ManacostDesc Manacost { get; private set; }

		[field: SerializeField]
		public CastingDesc Casting { get; private set; }

		[field: SerializeField]
		public CooldownDesc Cooldown { get; private set; }

		[field: SerializeReference]
		public IOperation ActiveOperation { get; private set; }

		[field: SerializeReference]
		public IAbilityTrigger[] Triggers { get; private set; }
	}
}
