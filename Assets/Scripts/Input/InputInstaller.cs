using Omniverse.Units.Client;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Input")]
	public class InputInstaller : ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private UnitControllerConfig UnitControllerConfig { get; set; }

		[field: SerializeField]
		private AbilityTargetRenderer AbilityTargetRenderer { get; set; }

		[field: SerializeField]
		private AbilityRangeRenderer AbilityRangeRenderer { get; set; }

		[field: SerializeField]
		private AbilityTrajectoryRenderer TrajectoryRenderer { get; set; }

		public void Install(IContainerBuilder builder)
		{
			var inputActions = new InputActions();
			builder.RegisterInstance(inputActions.Common);
			builder.RegisterInstance(inputActions.Abilities);

			inputActions.Abilities.Enable();
			inputActions.Common.Enable();

			builder.RegisterComponentInNewPrefab(AbilityTargetRenderer, Lifetime.Singleton);
			builder.RegisterComponentInNewPrefab(AbilityRangeRenderer, Lifetime.Singleton);
			builder.RegisterComponentInNewPrefab(TrajectoryRenderer, Lifetime.Singleton);

			builder.Register<EntityDetector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterEntryPoint<ErrorHandler>().AsSelf();
			builder.RegisterEntryPoint<AbilityController>().AsSelf();
	
			builder.RegisterEntryPoint<AbilityInputListener>().AsSelf();

			builder.Register<UnitSelector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.Register<UnitController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
				.WithParameter(UnitControllerConfig);
		}
	}
}
