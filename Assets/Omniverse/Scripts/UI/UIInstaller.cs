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
		
		[field: SerializeField]
		private UnitWidget UnitWidget { get; set; }
		
		public void Install(IContainerBuilder builder)
		{
			RegisterWidget(MiniMapWidget);
			RegisterWidget(UnitSelectorWidget);
			RegisterWidget(UnitWidget);

			void RegisterWidget<T>(T component) where T : Component
			{
				builder.RegisterComponentInNewPrefab(component, Lifetime.Singleton).UnderTransform(transform)
					.AsImplementedInterfaces().AsSelf();
			}
		}
	}
}
