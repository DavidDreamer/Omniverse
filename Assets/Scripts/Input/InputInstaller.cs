using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Input")]
	public class InputInstaller : ScriptableObject, IInstaller
	{
		public void Install(IContainerBuilder builder)
		{
			builder.Register<Detector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterEntryPoint<ErrorHandler>().AsSelf();

			builder.Register<Selector>(Lifetime.Singleton).AsSelf();
			builder.Register<UnitController>(Lifetime.Singleton).AsSelf();
			builder.RegisterEntryPoint<AbilityController>().AsSelf();

			builder.Register<InputController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
		}
	}
}
