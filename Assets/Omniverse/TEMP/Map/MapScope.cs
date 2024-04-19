using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class MapScope: LifetimeScope
	{
		[field: SerializeField]
		private MapSettings MapSettings { get; set; }

		[field: SerializeField]
		private MiniMap MiniMap { get; set; }

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(MapSettings);
			builder.RegisterComponentInNewPrefab(MiniMap, Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		}
	}
}
