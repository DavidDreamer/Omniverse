using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class ResourceBarWidget: MonoBehaviour, IInitializable
	{
		[field: SerializeField]
		private ResourceWidget ResourceWidgetPrefab { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		[Inject]
		private ResourceDesc[] ResourceDescs { get; set; }

		public void Initialize()
		{
			foreach (ResourceDesc desc in ResourceDescs)
			{
				ResourceWidget resourceWidget = ObjectResolver.Instantiate(ResourceWidgetPrefab, transform);
				resourceWidget.Initialize(desc);
			}
		}
	}
}
