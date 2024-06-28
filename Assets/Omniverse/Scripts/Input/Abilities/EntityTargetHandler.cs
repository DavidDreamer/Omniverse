using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using VContainer;

namespace Omniverse.Input
{
	public class EntityTargetHandler: TargetAbilityHandler
	{
		[Inject]
		private EntityDetector EntityDetector { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		protected override async UniTask GetTarget(Unit caster, CancellationToken token)
		{
			var entityTarget = (EntityTarget)Ability.Desc.Target;

			EntityDetector.ClearFilter();
			if (entityTarget.Resources.Length > 0)
			{
				EntityDetector.AddToFilter<ResourceSource>();
			}

			EntityDetector.AddToFilter<UnitPresenter>();

			bool inputProcessed;
			bool hasTarget;

			do
			{
				await UniTask.NextFrame(token);

				inputProcessed = CommonActions.Apply.WasPressedThisFrame();
				hasTarget = EntityDetector.Target != null;
			}
			while (!inputProcessed || !hasTarget);

			EntityDetector.SetDefaultDetectableType();
		}
	}
}
