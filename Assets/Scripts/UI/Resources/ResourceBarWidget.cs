using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.UI
{
	public class ResourceBarWidget : MonoBehaviour
	{
		[field: SerializeField]
		private ResourceWidget ResourceWidgetPrefab { get; set; }

		private List<ResourceWidget> ResourceWidgets { get; } = new();

		public void Awake()
		{
			var gameOptions = ECSUtils.GetSingletonManaged<GameOptions>();

			for (int i = 0; i < gameOptions.Resources.Length; i++)
			{
				ResourceDesc desc = gameOptions.Resources[i];
				ResourceWidget resourceWidget = Instantiate(ResourceWidgetPrefab, transform);
				resourceWidget.Initialize(desc, i);
				ResourceWidgets.Add(resourceWidget);
			}
		}

		public void LateUpdate()
		{
			foreach (ResourceWidget resourceWidget in ResourceWidgets)
			{
				resourceWidget.LateTick();
			}
		}
	}
}
