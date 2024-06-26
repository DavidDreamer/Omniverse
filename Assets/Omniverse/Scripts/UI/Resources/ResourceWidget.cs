using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ResourceWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Amount { get; set; }

		private Resource Resource { get; set; }

		public void Initialize(ResourceDesc resourceDesc)
		{
			Icon.sprite = resourceDesc.Icon;
		}
	}
}
