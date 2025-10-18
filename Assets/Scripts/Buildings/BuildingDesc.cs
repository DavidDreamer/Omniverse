using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Building")]
	public class BuildingDesc : ScriptableObject
	{
		[field: SerializeField]
		public Meta Meta { get; private set; }
	}
}
