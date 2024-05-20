using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UIInstaller: MonoBehaviour, IInstaller
	{
		[field: SerializeField]
		private MiniMapWidget MiniMapWidget { get; set; }
		
		[field: SerializeField]
		private UnitSelectorWidget UnitSelectorWidget { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			builder.RegisterComponentInNewPrefab(MiniMapWidget, Lifetime.Singleton).UnderTransform(transform)
				.AsImplementedInterfaces().AsSelf();
			
			builder.RegisterComponentInNewPrefab(UnitSelectorWidget, Lifetime.Singleton).UnderTransform(transform)
				.AsImplementedInterfaces().AsSelf();
		}
	}
}
