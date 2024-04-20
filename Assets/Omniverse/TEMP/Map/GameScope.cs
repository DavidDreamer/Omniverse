using System.Linq;
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
		private MiniMap MiniMap { get; set; }

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(GameSettings);
			builder.RegisterInstance(GameSettings.MapSettings);
			
			builder.RegisterComponentInNewPrefab(MiniMap, Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			
			builder.RegisterEntryPoint<FactionManager>().AsSelf().WithParameter(GameSettings.Factions.ToList());
		}
	}
}
