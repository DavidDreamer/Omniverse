using Omniverse;
using Omniverse.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationScope: LifetimeScope
{
	[field: SerializeField]
	private OmniverseInstaller OmniverseInstaller { get; set; }

	protected override void Configure(IContainerBuilder builder)
	{
		OmniverseInstaller.Install(builder);
	}
}
