using TMPro;
using UnityEngine;

namespace Omniverse.UI
{
	public class AbilityTooltipWidget : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI Name { get; set; }

		public void Bind(MetaData metaData)
		{
			Name.text = metaData.Name.ToString().ToUpper();
		}
	}
}
