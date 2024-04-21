using Omniverse;
using Omniverse.Camera;
using Omniverse.Cameras;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationScope: LifetimeScope
{
	[field: SerializeField]
	private OmniverseInstaller OmniverseInstaller { get; set; }

	[field: SerializeField]
	private UnitSelectorConfig UnitSelectorConfig { get; set; }

	[field: SerializeField]
	private UnitControllerConfig UnitControllerConfig { get; set; }
	
	protected override void Configure(IContainerBuilder builder)
	{
		OmniverseInstaller.Install(builder);

		builder.Register<UnitSelector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
			.WithParameter(UnitSelectorConfig);
		builder.Register<UnitController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
			.WithParameter(UnitControllerConfig);
	}
}
