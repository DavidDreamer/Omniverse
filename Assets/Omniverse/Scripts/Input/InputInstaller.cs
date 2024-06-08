using Bonecrusher.Abilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Installer/Input")]
	public class InputInstaller: ScriptableObject, IInstaller
	{
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

			builder.RegisterEntryPoint<AbilityController>().AsSelf();
			builder.RegisterEntryPoint<UnitTargetController>().AsSelf();
			builder.RegisterEntryPoint<PointAbilityController>().AsSelf();
			builder.RegisterEntryPoint<TrajectoryAbilityController>().AsSelf();

			builder.RegisterEntryPoint<AbilityInputListener>().AsSelf();
		}
	}
}
