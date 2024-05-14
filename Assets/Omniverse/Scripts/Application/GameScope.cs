using System.Linq;
using Omniverse.Cameras;
using Omniverse.Mapping;
using Omniverse.FogOfWar;
using Omniverse.FogOfWar.Rendering;
using Omniverse.UI;
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

		[field: SerializeField]
		private FogOfWarRenderer FogOfWarRenderer { get; set; }
		
		[field: SerializeField]
		private UIInstaller UIInstaller { get; set; }
		
		[field: SerializeField]
		public CameraController CameraController { get; set; }
		
		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(GameSettings);
			builder.RegisterInstance(GameSettings.MapSettings);
			builder.RegisterInstance(GameSettings.Factions);

			if (Map != null)
			{
				builder.RegisterComponentInNewPrefab(Map, Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			}

			if (GameSettings.FogOfWarMode is not Mode.None)
			{
				builder.Register<FogOfWarManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
				builder.RegisterComponentInNewPrefab(FogOfWarRenderer, Lifetime.Singleton).AsImplementedInterfaces()
					.AsSelf();
			}
			
			builder.RegisterEntryPoint<FactionManager>().AsSelf().WithParameter(GameSettings.Factions.ToList());

			builder.RegisterInstance(CameraController).AsImplementedInterfaces().AsSelf();
			
			UIInstaller.Install(builder);
		}
	}
}
