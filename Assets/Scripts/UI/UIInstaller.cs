using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.UI
{
	public class UIInstaller : MonoBehaviour, IInstaller
	{
		[field: SerializeField]
		private UIStyle Style { get; set; }

		[field: SerializeField]
		private ResourceBarWidget ResourceBarWidget { get; set; }

		[field: SerializeField]
		private MiniMapWidget MiniMapWidget { get; set; }

		[field: SerializeField]
		private UnitSelectorWidget UnitSelectorWidget { get; set; }

		[field: SerializeField]
		private UnitWidget UnitWidget { get; set; }

		[field: SerializeField]
		private EffectWidget EffectWidget { get; set; }

		public void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(Style);

			RegisterWidget(ResourceBarWidget);
			RegisterWidget(MiniMapWidget);
			RegisterWidget(UnitSelectorWidget);
			RegisterWidget(UnitWidget);

			builder.RegisterComponentInNewPrefab(EffectWidget, Lifetime.Transient);

			void RegisterWidget<T>(T component) where T : Component
			{
				builder.RegisterComponentInNewPrefab(component, Lifetime.Singleton).UnderTransform(transform)
					.AsImplementedInterfaces().AsSelf();
			}
		}
	}
}
