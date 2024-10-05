using Omniverse.Items;
using Omniverse.Mapping;
using Omniverse.Rendering;
using Omniverse.UI;
using Omniverse.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameScope : LifetimeScope
	{
		[field: SerializeField]
		private GameSettings GameSettings { get; set; }

		[field: SerializeField]
		private Map Map { get; set; }

		[field: SerializeField]
		private UIInstaller UIInstaller { get; set; }

		[field: SerializeField]
		private RenderingInstaller RenderingInstaller { get; set; }

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(GameSettings);
			builder.RegisterInstance(GameSettings.MapSettings);
			builder.RegisterInstance(GameSettings.Factions);
			builder.RegisterInstance(GameSettings.Resources);

			if (Map != null)
			{
				builder.RegisterComponentInNewPrefab(Map, Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			}

			if (GameSettings.FogOfWarSettings.Enabled)
			{
				builder.RegisterInstance(GameSettings.FogOfWarSettings);
				builder.Register<FogOfWar.Manager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
			}

			builder.Register<PrefabPool<Unit>>(Lifetime.Singleton);
			builder.Register<PrefabPool<Item>>(Lifetime.Singleton);

			builder.RegisterEntryPoint<Items.Manager>().AsSelf();
			builder.RegisterEntryPoint<Units.Manager>().AsSelf();

			builder.RegisterEntryPoint<FactionManager>().AsSelf();
			builder.RegisterEntryPoint<ResourceExtractionHadler>().AsSelf();

			RenderingInstaller.Install(builder);
			UIInstaller.Install(builder);
		}
	}
}
