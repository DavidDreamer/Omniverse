using Omniverse.Entities.Units.Client;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Input")]
	public class InputInstaller: ScriptableObject, IInstaller
	{
		[field: SerializeField]
		private UnitSelectorConfig UnitSelectorConfig { get; set; }

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

			builder.RegisterEntryPoint<ErrorHandler>().AsSelf();
			builder.RegisterEntryPoint<AbilityHandler>().AsSelf();
			builder.RegisterEntryPoint<EntityTargetHandler>().AsSelf();
			builder.RegisterEntryPoint<PointAbilityHandler>().AsSelf();
			//builder.RegisterEntryPoint<TrajectoryAbilityHandler>().AsSelf();
			builder.RegisterEntryPoint<AbilityHandlerResolver>().AsSelf();

			builder.RegisterEntryPoint<AbilityInputListener>().AsSelf();
			
			builder.Register<EntityDetector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			builder.Register<EntityOutliner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			builder.Register<UnitSelector>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
				.WithParameter(UnitSelectorConfig);
			builder.Register<UnitController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf()
				.WithParameter(UnitControllerConfig);
		}
	}
}
