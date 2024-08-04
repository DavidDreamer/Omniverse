using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Abilities;
using Omniverse.Units;
using VContainer;

namespace Omniverse.Input
{
	public class EntityTargetHandler : TargetAbilityHandler
	{
		[Inject]
		private EntityDetector EntityDetector { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		protected override async UniTask GetTarget(Unit caster, CancellationToken token)
		{
			var entityTarget = (EntityTarget)Ability.Target;

			EntityDetector.ClearFilter();

			if (entityTarget.Desc.ResourceSources != null)
			{
				EntityDetector.AddToFilter<ResourceSource>();
			}

			if (entityTarget.Desc.Units != null)
			{
				EntityDetector.AddToFilter<Unit>();
			}

			bool inputProcessed;
			bool hasTarget;

			do
			{
				await UniTask.NextFrame(token);

				inputProcessed = CommonActions.Apply.WasPressedThisFrame();
				hasTarget = EntityDetector.Target != null;
			}
			while (!inputProcessed || !hasTarget);

			entityTarget.Value = EntityDetector.Target;

			EntityDetector.SetDefaultDetectableType();
		}
	}
}
