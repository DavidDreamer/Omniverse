using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class AbilityTooltipWidget : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		public void Bind(string abilityName)
		{
			Name.text = abilityName.ToUpper();
		}
	}
}
