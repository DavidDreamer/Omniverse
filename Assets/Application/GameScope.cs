using Omniverse.Items;
using Omniverse.Mapping;
using Omniverse.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameScope : LifetimeScope
	{
		[field: SerializeField]
		private OmniverseInstaller OmniverseInstaller { get; set; }

		[field: SerializeField]
		private GameSettings GameSettings { get; set; }

		[field: SerializeField]
		private UIInstaller UIInstaller { get; set; }

		protected override void Configure(IContainerBuilder builder)
		{
			OmniverseInstaller.Install(builder);

			builder.RegisterInstance(GameSettings);
			builder.RegisterInstance(GameSettings.Factions);
			builder.RegisterInstance(GameSettings.Resources);

			builder.Register<PrefabPool<UnitObsolete>>(Lifetime.Singleton);
			builder.Register<PrefabPool<Item>>(Lifetime.Singleton);

			builder.RegisterEntryPoint<ItemManager>().AsSelf();
			builder.RegisterEntryPoint<TempNameManager>().AsSelf();
			builder.RegisterEntryPoint<UnitManager>().AsSelf();

			builder.RegisterEntryPoint<FactionManager>().AsSelf();
			builder.RegisterEntryPoint<ResourceExtractionHadler>().AsSelf();

			UIInstaller.Install(builder);

			builder.Register<GameDirector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		}
	}
}
