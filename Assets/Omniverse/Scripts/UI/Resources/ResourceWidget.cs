using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class ResourceWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Amount { get; set; }

		private ResourceDesc ResourceDesc { get; set; }

		[Inject]
		private FactionManager FactionManager { get; set; }
		
		[Inject]
		private Player Player { get; set; }
		
		public void Initialize(ResourceDesc resourceDesc)
		{
			ResourceDesc = resourceDesc;
			Icon.sprite = resourceDesc.Icon;
		}

		public void LateTick()
		{
			Resource resource = FactionManager.Factions[Player.FactionID].Resources[ResourceDesc];
			Amount.text = resource.Amount.Value.ToString();
		}
	}
}
