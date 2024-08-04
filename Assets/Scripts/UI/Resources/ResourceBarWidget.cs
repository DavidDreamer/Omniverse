using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class ResourceBarWidget : MonoBehaviour, IInitializable, ILateTickable
	{
		[field: SerializeField]
		private ResourceWidget ResourceWidgetPrefab { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		[Inject]
		private ResourceDesc[] ResourceDescs { get; set; }

		private List<ResourceWidget> ResourceWidgets { get; } = new();

		public void Initialize()
		{
			foreach (ResourceDesc desc in ResourceDescs)
			{
				ResourceWidget resourceWidget = ObjectResolver.Instantiate(ResourceWidgetPrefab, transform);
				resourceWidget.Initialize(desc);
				ResourceWidgets.Add(resourceWidget);
			}
		}

		public void LateTick()
		{
			foreach (ResourceWidget resourceWidget in ResourceWidgets)
			{
				resourceWidget.LateTick();
			}
		}
	}
}
