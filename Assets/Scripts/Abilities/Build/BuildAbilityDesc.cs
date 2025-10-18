using UnityEngine;

namespace Omniverse.Abilities
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Ability/Build")]
	public class BuildAbilityDesc : ScriptableObject
	{
		[field: SerializeField]
		public Meta Meta { get; private set; }

		[field: SerializeField]
		public GameObject[] Buildings { get; private set; }
	}
}
