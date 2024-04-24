using System.Linq;
using Omniverse.Mapping;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	public class GameScope: LifetimeScope
	{
		[field: SerializeField]
		private GameSettings GameSettings { get; set; }

		[field: SerializeField]
		private Map Map { get; set; }

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(GameSettings);
			builder.RegisterInstance(GameSettings.MapSettings);

			if (Map != null)
			{
				builder.RegisterComponentInNewPrefab(Map, Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			}

			builder.RegisterEntryPoint<FactionManager>().AsSelf().WithParameter(GameSettings.Factions.ToList());
		}
	}
}
