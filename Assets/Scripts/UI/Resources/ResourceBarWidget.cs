using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class ResourceBarWidget : Widget
	{
		[field: SerializeField]
		private ResourceWidget ResourceWidgetPrefab { get; set; }

		private List<ResourceWidget> ResourceWidgets { get; } = new();

		public override void Initialize(EntityManager entityManager)
		{
			base.Initialize(entityManager);

			var gameOptions = entityManager.GetSingletonManaged<GameOptions>();

			for (int i = 0; i < gameOptions.Resources.Length; i++)
			{
				ResourceDesc desc = gameOptions.Resources[i];
				ResourceWidget resourceWidget = Instantiate(ResourceWidgetPrefab, transform);
				resourceWidget.Initialize(desc, i);
				ResourceWidgets.Add(resourceWidget);
			}
		}

		public override void Tick()
		{
			foreach (ResourceWidget resourceWidget in ResourceWidgets)
			{
				resourceWidget.Tick(EntityManager);
			}
		}
	}
}
