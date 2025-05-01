using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ResourceWidget : MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private TextMeshProUGUI Amount { get; set; }

		private ResourceDesc ResourceDesc { get; set; }

		private int Index { get; set; }

		public void Initialize(ResourceDesc resourceDesc, int index)
		{
			ResourceDesc = resourceDesc;
			Icon.sprite = resourceDesc.Icon;
			Index = index;
		}

		public void Tick(EntityManager entityManager)
		{
			var player = entityManager.GetSingleton<Player>();
			var factionsData = entityManager.GetSingleton<FactionsData>();

			int amount = factionsData.Resources[player.FactionID][Index];
			Amount.text = amount.ToString();
		}
	}
}
