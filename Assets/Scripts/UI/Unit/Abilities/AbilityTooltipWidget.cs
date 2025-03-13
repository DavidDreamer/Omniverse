using Omniverse.Abilities;
using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class AbilityTooltipWidget : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		public void Bind(AbilityObsolete ability)
		{
			Name.text = ability.Desc.Meta.Name.ToUpper();
		}
	}
}
