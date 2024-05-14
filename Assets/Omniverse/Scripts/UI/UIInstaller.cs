using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UIInstaller: MonoBehaviour, IInstaller
	{
		[field: SerializeField]
		private MiniMapWidget MiniMapWidget { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			builder.RegisterComponentInNewPrefab(MiniMapWidget, Lifetime.Singleton).UnderTransform(transform)
				.AsImplementedInterfaces().AsSelf();
		}
	}
}
